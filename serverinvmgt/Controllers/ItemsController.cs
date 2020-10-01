using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using serverinvmgt.operations;
using serverinvmgt.Models;
using System.Web;

namespace serverinvmgt.Controllers
{
    public class ItemsController : ApiController
    {

        [HttpGet]
        [Route("api/items/get")]
        public IHttpActionResult GetItems()
        {
            Response res = new Response();
            clsItemsOps clsItemsOps = new clsItemsOps();
            List<clsItems> lstItems = new List<clsItems>();
            try
            {
                #region Validates Input request 

                if (!ModelState.IsValid)
                {
                    var errorDesciption = (from item in ModelState
                                           where item.Value.Errors.Any()
                                           select item.Value.Errors[0].ErrorMessage).ToList();

                    res.Error = new ErrorResponse("ERR.PARAMETERS.INVALID", errorDesciption.First());
                    res.Data = new object();

                    return NotFound();
                }
                #endregion

                lstItems = clsItemsOps.getItems();
                if (lstItems == null)
                {
                    res.Error = new ErrorResponse("ERR.IN.ADD", "Error in getting item details.");
                    res.Data = new object();
                    return InternalServerError();
                }
                else
                {
                    res.Data = new object();
                    res.Data = lstItems;
                    return Ok(res);
                }
            }
            catch (Exception ex) {
                return InternalServerError();
            }
        }
        [HttpPost]
        [Route("api/items/add")]
        public IHttpActionResult AddItem() 
        {
            Response res = new Response();
            clsItemsOps clsItemsOps = new clsItemsOps();
            clsItems objItem = new clsItems();
            HttpPostedFile file = null;
            string Return = string.Empty;
            try
            {
               // form-data & image as a file can be accessed using this way only. Or else Request class. 

                objItem.Name = HttpContext.Current.Request.Params["Name"];
                objItem.Price = Convert.ToDouble(HttpContext.Current.Request.Params.Get("Price")); 
                objItem.Desc = HttpContext.Current.Request.Params.Get("Desc");

                Return = clsItemsOps.addItem(objItem);

                if (string.IsNullOrWhiteSpace(Return)) 
                {
                    res.Error = new ErrorResponse("ERR.IN.ADD", "Error in adding new item details.");
                    res.Data = new object();
                    return InternalServerError();
                }
                else
                {
                    if (HttpContext.Current.Request.Files.Count > 0)
                    {
                        file = HttpContext.Current.Request.Files.Get("file");
                        clsItemsOps.subSaveImage(file, Return);
                    }
                    res.Data = new object();
                    res.Data = Return;
                    return Ok(res);
                }
            }
            catch (Exception ex)
            {
                return InternalServerError();
            }
        }

        [HttpPatch]
        [Route("api/items/edit")]
        public IHttpActionResult EditItem()
        {
            Response res = new Response();
            clsItemsOps clsItemsOps = new clsItemsOps();
            clsItems objItem = new clsItems();
            string imgName = "0";
            HttpPostedFile file = null;
            try
            {
                imgName = HttpContext.Current.Request.Params["Id"];
                objItem.Id = Convert.ToInt32(imgName); 
                objItem.Name = HttpContext.Current.Request.Params["Name"];
                objItem.Price = Convert.ToDouble(HttpContext.Current.Request.Params.Get("Price"));
                objItem.Desc = HttpContext.Current.Request.Params.Get("Desc");
             
                bool Return = clsItemsOps.editItembyId(objItem);

                if (!Return)
                {
                    res.Error = new ErrorResponse("ERR.IN.ADD", "Error in updating item details.");
                    res.Data = new object();
                    return InternalServerError();
                }
                else
                {
                    if (HttpContext.Current.Request.Files.Count > 0)
                    {
                        file = HttpContext.Current.Request.Files.Get("file");
                        clsItemsOps.subSaveImage(file, imgName);
                    }

                    res.Data = new object();
                    res.Data = Return;
                    return Ok(res);
                }
            }
            catch (Exception ex)
            {
                return InternalServerError();
            }
        }

        [HttpDelete]
        [Route("api/items/delete")]
        public IHttpActionResult DelItem(long id)
        {
            Response res = new Response();
            clsItemsOps clsItemsOps = new clsItemsOps();

            try
            {
                bool Return = clsItemsOps.deleteItembyId(id);

                if (!Return)
                {
                    res.Error = new ErrorResponse("ERR.IN.ADD", "Error in deleting item details.");
                    res.Data = new object();
                    return InternalServerError();
                }
                else
                {
                    res.Data = new object();
                    res.Data = Return;
                    return Ok(res);
                }
            }
            catch (Exception ex)
            {
                return InternalServerError();
            }
        }

        [HttpGet]
        [Route("api/items/getitem")]
        public IHttpActionResult GetItem(long Id)
        {
            Response res = new Response();
            clsItemsOps clsItemsOps = new clsItemsOps();
            clsItems objItem = new clsItems();
            try
            {
                #region Validates Input request 

                if (!ModelState.IsValid)
                {
                    var errorDesciption = (from item in ModelState
                                           where item.Value.Errors.Any()
                                           select item.Value.Errors[0].ErrorMessage).ToList();

                    res.Error = new ErrorResponse("ERR.PARAMETERS.INVALID", errorDesciption.First());
                    res.Data = new object();

                    return NotFound();
                }
                #endregion

                objItem = clsItemsOps.getItembyId(Id); 
                if (objItem == null)
                {
                    res.Error = new ErrorResponse("ERR.IN.ADD", "Error in getting item details.");
                    res.Data = new object();
                    return InternalServerError();
                }
                else
                {
                    res.Data = new object();
                    res.Data = objItem;
                    return Ok(res);
                }
            }
            catch (Exception ex)
            {
                return InternalServerError();
            }
        }

    }
}
