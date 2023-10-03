using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace TestAPI.Controllers
{
    public class ValuesController : ApiController
    {
        //// GET api/values
        //public IEnumerable<string> Get()
        //{
        //    return new string[] { "value1", "value2" };
        //}

        // GET api/values
        public HttpResponseMessage Get()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("DeptID");
            dt.Columns.Add("DeptName");

            dt.Rows.Add(1, "Phisic");
            dt.Rows.Add(2, "Math");

            return Request.CreateResponse(HttpStatusCode.OK, dt);
        }


        // GET api/values/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/values
        public void Post([FromBody] string value)
        {
        }

        // PUT api/values/5
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5
        public void Delete(int id)
        {
        }
    }
}
