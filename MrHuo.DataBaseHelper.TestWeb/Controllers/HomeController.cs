using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MrHuo.DataBaseHelper;
using System.Data;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using System.Text.RegularExpressions;
using System.Text;

namespace MrHuo.DataBaseHelper.TestWeb.Controllers
{
    public class HomeController : Controller
    {
        static Interfaces.IDataBaseConstructHelper dataBaseConstructHelper = null;
        // GET: Home
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public JsonResult TestConnect(string dbType = "", string dbConnString = "")
        {
            dataBaseConstructHelper = null;
            if (dbType.IsNullOrEmpty())
            {
                return Json(new RestResult<string>()
                {
                    msg = $"参数[{nameof(dbType)}]不能为空！"
                });
            }
            if (dbConnString.IsNullOrEmpty())
            {
                return Json(new RestResult<string>()
                {
                    msg = $"参数[{nameof(dbConnString)}]不能为空！"
                });
            }
            var dataBaseType = (DataBaseType)Enum.Parse(typeof(DataBaseType), dbType);
            var testResult = DbConnectionFactory.TestConnection(dataBaseType, dbConnString);
            if (testResult.Success)
            {
                dataBaseConstructHelper = DataBaseConstructHelperFactory.CreateDataBaseConstructHelper(dataBaseType, dbConnString);
            }
            return Json(new RestResult<string>()
            {
                ret = testResult.Success,
                msg = testResult.ErrorMessage
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public async Task<MyJsonResult> GetTables()
        {
            var tables = await dataBaseConstructHelper.GetTables();
            return Json(new RestResult<IEnumerable<Objects.Table>>()
            {
                ret = true,
                msg = $"Getted {tables.Count()} Tables",
                data = tables
            });
        }

        [HttpPost]
        public async Task<MyJsonResult> GetColumns(string table = "")
        {
            var columns = await dataBaseConstructHelper.GetColumns(table);
            return Json(new RestResult<IEnumerable<Objects.Column>>()
            {
                ret = true,
                msg = $"Getted {columns.Count()} Columns",
                data = columns
            });
        }

        [HttpPost]
        public async Task<MyJsonResult> GetProcedures()
        {
            var procedures = await dataBaseConstructHelper.GetProcedures();
            return Json(new RestResult<IEnumerable<Objects.Procedure>>()
            {
                ret = true,
                msg = $"Getted {procedures.Count()} Procedures",
                data = procedures
            });
        }

        [HttpPost]
        public async Task<MyJsonResult> GetViews()
        {
            var views = await dataBaseConstructHelper.GetViews();
            return Json(new RestResult<IEnumerable<Objects.View>>()
            {
                ret = true,
                msg = $"Getted {views.Count()} Views",
                data = views
            });
        }

        [HttpPost]
        public async Task<MyJsonResult> GetFunctions()
        {
            var functions = await dataBaseConstructHelper.GetFunctions();
            return Json(new RestResult<IEnumerable<Objects.Function>>()
            {
                ret = true,
                msg = $"Getted {functions.Count()} Functions",
                data = functions
            });
        }

        [HttpPost]
        public async Task<MyJsonResult> GetForeignKeys()
        {
            var foreignKeys = await dataBaseConstructHelper.GetForeignKeys();
            return Json(new RestResult<IEnumerable<Objects.ForeignKey>>()
            {
                ret = true,
                msg = $"Getted {foreignKeys.Count()} ForeignKeys",
                data = foreignKeys
            });
        }

        [HttpPost]
        public async Task<MyJsonResult> GetProcedureColumns(string procedure="")
        {
            var procedureColumns = await dataBaseConstructHelper.GetProcedureColumns(procedure);
            return Json(new RestResult<IEnumerable<Objects.ProcedureColumn>>()
            {
                ret = true,
                msg = $"Getted {procedureColumns.Count()} ProcedureColumns",
                data = procedureColumns
            });
        }

        [HttpPost]
        public async Task<MyJsonResult> GetProcedureScript(string procedure = "")
        {
            var procedureScript = await dataBaseConstructHelper.GetProcedureScript(procedure);
            return Json(new RestResult<string>()
            {
                ret = true,
                msg = $"Getted {procedure} Script",
                data = procedureScript
            });
        }

        [HttpPost]
        public async Task<MyJsonResult> GetViewColumns(string view = "")
        {
            var viewColumns = await dataBaseConstructHelper.GetViewColumns(view);
            return Json(new RestResult<IEnumerable<Objects.ViewColumn>>()
            {
                ret = true,
                msg = $"Getted {viewColumns.Count()} ViewColumns",
                data = viewColumns
            });
        }

        [HttpPost]
        public async Task<MyJsonResult> GetViewScript(string view = "")
        {
            var viewScript = await dataBaseConstructHelper.GetViewScript(view);
            return Json(new RestResult<string>()
            {
                ret = true,
                msg = $"Getted {view} Script",
                data = viewScript
            });
        }

        [HttpPost]
        public async Task<MyJsonResult> GetFunctionScript(string function = "")
        {
            var functionScript = await dataBaseConstructHelper.GetFunctionScript(function);
            return Json(new RestResult<string>()
            {
                ret = true,
                msg = $"Getted {function} Script",
                data = functionScript
            });
        }

        /// <summary>
        /// 生成 JsonResult
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        private new MyJsonResult Json(object data)
        {
            return new MyJsonResult
            {
                Data = data,
                ContentEncoding = Encoding.UTF8,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }
    }

    public class MyJsonResult : JsonResult
    {
        /// <summary>
        /// 格式化字符串
        /// </summary>
        public string DateTimeFormat { get; set; } = "yyyy-MM-dd HH:mm:ss";
        public override void ExecuteResult(ControllerContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }

            HttpResponseBase response = context.HttpContext.Response;
            if (string.IsNullOrEmpty(this.ContentType))
            {
                response.ContentType = this.ContentType;
            }
            else
            {
                response.ContentType = "application/json";
            }
            if (this.ContentEncoding != null)
            {
                response.ContentEncoding = this.ContentEncoding;
            }

            if (this.Data != null)
            {
                JavaScriptSerializer jss = new JavaScriptSerializer();
                string jsonString = jss.Serialize(Data);
                string p = @"\\/Date\((\d+)\)\\/";
                MatchEvaluator matchEvaluator = new MatchEvaluator(this.ConvertJsonDateToDateString);
                Regex reg = new Regex(p);
                jsonString = reg.Replace(jsonString, matchEvaluator);
                response.Write(jsonString);
            }
        }
        /// <summary>  
        /// 将Json序列化的时间由/Date(1294499956278)转为字符串 .
        /// </summary>  
        /// <param name="m">正则匹配</param>
        /// <returns>格式化后的字符串</returns>
        private string ConvertJsonDateToDateString(Match m)
        {
            string result = string.Empty;
            DateTime dt = new DateTime(1970, 1, 1);
            dt = dt.AddMilliseconds(long.Parse(m.Groups[1].Value));
            dt = dt.ToLocalTime();
            result = dt.ToString(this.DateTimeFormat);
            return result;
        }
    }

    class RestResult<T>
    {
        public bool ret { get; set; }
        public string msg { get; set; }
        public T data { get; set; } = default(T);
    }

    static class Extensions
    {
        public static bool IsNullOrEmpty(this string str)
        {
            return string.IsNullOrWhiteSpace(str + string.Empty);
        }
    }
}