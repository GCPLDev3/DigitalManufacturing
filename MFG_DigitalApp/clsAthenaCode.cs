using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using Amazon;
using Amazon.Athena;
using Amazon.Athena.Model;

namespace MFG_DigitalApp
{
    public class clsAthenaCode
    {
        public string Client = "";
        public String StrQuery = "";
        private const String ATHENA_TEMP_PATH = "s3://gcpl-in-as1-prod-raw-mindsphere-iot/";
        private const String ATHENA_DB = "gcpl_dl_mindsphere_iot";

        public static List<Dictionary<String, String>> runQuery(String StrQuery)
        {
            var client = new AmazonAthenaClient("AKIAWR5GGX5MW7QKSAWV", "jEdL+v3zQa8oTg/3Pkx0nd9+y3j5ZUP9AULNRW/H", Amazon.RegionEndpoint.APSouth1);
            QueryExecutionContext qContext = new QueryExecutionContext();
            qContext.Database = ATHENA_DB;
            ResultConfiguration resConf = new ResultConfiguration();
            resConf.OutputLocation = ATHENA_TEMP_PATH;
            List<Dictionary<String, String>> items = new List<Dictionary<String, String>>();

            try
            {
                /* Execute a simple query on a table */
                StartQueryExecutionRequest qReq = new StartQueryExecutionRequest()
                {
                    QueryString = StrQuery,
                    QueryExecutionContext = qContext,
                    ResultConfiguration = resConf
                };

                /* Executes the query in an async manner */
                StartQueryExecutionResponse qRes = client.StartQueryExecution(qReq); // await client.StartQueryExecutionAsync(qReq);
                                                                                     /* Call internal method to parse the results and return a list of key/value dictionaries */
                items = getQueryExecution(client, qRes.QueryExecutionId); //await getQueryExecution(client, qRes.QueryExecutionId);
            }
            catch (InvalidRequestException e)
            {
                Console.WriteLine("Run Error: {0}", e.Message);
            }

            return items;
        }

        public static List<Dictionary<String, String>> getQueryExecution(IAmazonAthena client, String id)
        {
            List<Dictionary<String, String>> items = new List<Dictionary<String, String>>();
            GetQueryExecutionResponse results = null;
            QueryExecution q = null;
            /* Declare query execution request object */
            GetQueryExecutionRequest qReq = new GetQueryExecutionRequest()
            {
                QueryExecutionId = id
            };
            /* Poll API to determine when the query completed */
            do
            {
                try
                {
                    results = client.GetQueryExecution(qReq); //await client.GetQueryExecutionAsync(qReq);
                    q = results.QueryExecution;
                    Console.WriteLine("Status: {0}... {1}", q.Status.State, q.Status.StateChangeReason);

                    Console.Write("<script>console.log('" + q.Status.StateChangeReason + "');</script>");

                    //await Task.Delay(5000); //Wait for 5sec before polling again
                }
                catch (InvalidRequestException e)
                {
                    Console.WriteLine("GetQueryExec Error: {0}", e.Message);
                }
            } while (q.Status.State == "RUNNING" || q.Status.State == "QUEUED");

            Console.WriteLine("Data Scanned for {0}: {1} Bytes", id, q.Statistics.DataScannedInBytes);

            /* Declare query results request object */
            GetQueryResultsRequest resReq = new GetQueryResultsRequest()
            {
                QueryExecutionId = id,
                MaxResults = 10
            };

            GetQueryResultsResponse resResp = null;
            /* Page through results and request additional pages if available */
            do
            {
                resResp = client.GetQueryResults(resReq); //await client.GetQueryResultsAsync(resReq);
                                                          /* Loop over result set and create a dictionary with column name for key and data for value */
                foreach (Row row in resResp.ResultSet.Rows)
                {
                    Dictionary<String, String> dict = new Dictionary<String, String>();
                    for (var i = 0; i < resResp.ResultSet.ResultSetMetadata.ColumnInfo.Count; i++)
                    {
                        dict.Add(resResp.ResultSet.ResultSetMetadata.ColumnInfo[i].Name, row.Data[i].VarCharValue);
                    }
                    items.Add(dict);
                }

                if (resResp.NextToken != null)
                {
                    resReq.NextToken = resResp.NextToken;
                }
            } while (resResp.NextToken != null);

            /* Return List of dictionary per row containing column name and value */
            return items;
        }
    }
}