var moduleCommon = (function(_public, $) {

    var _private = {

        //todo: any private methods here

    };

    var _public = {

        Logger: {
          
            Log: function() {
                false && console.log(this, arguments);
            }
        },
    
        //all common functions will be exposed from moduleCommon.Common.<function name>
        Common: {

            //common ajax call entrypoint.
            AjaxCall: function (settings) {

                $.ajax({
                    url: settings.url,
                    type: settings.type,
                    data: settings.data,
                    dataType: settings.dataType,
                    contentType: settings.contentType,
                    headers: settings.headers,
                    mimeType: settings.mimeType,
                    statusCode: settings.statusCode,

                    beforeSend: function() {
                        _public.Logger.Log(this, arguments);

                        if ($.isFunction(settings.beforeSend)) {
                            settings.beforeSend.apply(this, arguments);
                        }
                    },

                    success: function () {
                        _public.Logger.Log(this, arguments);

                        if ($.isFunction(settings.success)) {
                            settings.success.apply(this, arguments);
                        }
                    },

                    error: function () {
                        _public.Logger.Log(this, arguments);

                        if ($.isFunction(settings.error)) {
                            settings.error.apply(this, arguments);
                        }
                    }
                });
            }
        }
    };


    return _public;

})(moduleCommon || {}, jQuery);
