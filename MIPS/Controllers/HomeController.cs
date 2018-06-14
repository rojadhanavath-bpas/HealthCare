using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using System.IO;
using ExcelDataReader;


namespace MIPS.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

               
        public ActionResult Quality()
        {

            string path = Server.MapPath("../Excel Files/2018-Measure-List-EditedVersion.xlsx");
            FileStream test = new FileStream(path, FileMode.Open, FileAccess.Read);           
            Stream stream = test;            
            IExcelDataReader reader = null;
            reader = ExcelReaderFactory.CreateOpenXmlReader(stream);
                      
            DataSet result = reader.AsDataSet(new ExcelDataSetConfiguration()
            {
                ConfigureDataTable = (_) => new ExcelDataTableConfiguration()
                {
                    UseHeaderRow = true,
                    ReadHeaderRow = (rowReader) =>
                    {
                        rowReader.Read();
                    }


                }
            });
            reader.Close();
            
            return View(result.Tables[0]);


        }


        public ActionResult QualityBenchMarks()
        {

            string path = Server.MapPath("../Excel Files/2018-Measure-List-EditedVersion.xlsx");
            FileStream test = new FileStream(path, FileMode.Open, FileAccess.Read);
            Stream stream = test;
            IExcelDataReader reader = null;
            reader = ExcelReaderFactory.CreateOpenXmlReader(stream);

            DataSet result = reader.AsDataSet(new ExcelDataSetConfiguration()
            {
                ConfigureDataTable = (_) => new ExcelDataTableConfiguration()
                {
                    UseHeaderRow = true,
                    ReadHeaderRow = (rowReader) =>
                    {
                        rowReader.Read();
                    }


                }
            });
            reader.Close();

            return View(result.Tables[0]);


        }

    }

}



        