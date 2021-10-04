using System;
using System.Reflection;
using System.Text;
using System.Web;


namespace Clippy.UrlParamEncoder{
    class UrlParamEncoder {
        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();

        public static string ToUrlParams(object obj) {
            StringBuilder queryBuilder = new StringBuilder();
            bool hasParams = false;

            Type t = obj.GetType();
            foreach (PropertyInfo prop in t.GetProperties()) {
                UrlParamAttribute atr = (UrlParamAttribute) Attribute.GetCustomAttribute(prop, typeof(UrlParamAttribute));
                if (atr == null) {
                    //No attribute found
                    Logger.Trace($"No UrlParamAttribute found for property {prop.Name}; skipping...");
                    continue;
                } else {
                    //We have an attribute, so generate the query string
                    string paramName = atr.Name;
                    string val = $"{prop.GetValue(obj)}";
                    if (val == null || val == "") {
                        //Value is empty, so we can discard it
                        continue;
                    } else {
                        string encodedParamName = HttpUtility.UrlEncode(paramName);
                        string encodedVal = HttpUtility.UrlEncode(val);
                        if (hasParams) {
                            //This is not the first param, so start with an `&`
                            queryBuilder.AppendFormat("&{0}={1}", encodedParamName, encodedVal);
                        } else {
                            //This is the first param, so take note of it and start with `?`
                            hasParams = true;
                            queryBuilder.AppendFormat("?{0}={1}", encodedParamName, encodedVal);
                        }
                    }
                }
            }

            string queryString = queryBuilder.ToString();
            Logger.Debug($"Generated query string {queryString}");

            return queryString;
        }


    }
}
