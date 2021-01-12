using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MFG_DigitalApp.Log
{
    public interface ILogger
    {
        void Debug(string message);
        void Error(string message);
        void Error(string message, Exception exception);
        void Error(Exception exception);
        void Warn(string message);
        void Info(string message);
    }
}
