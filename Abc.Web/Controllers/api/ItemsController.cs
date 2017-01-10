using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Reflection;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace Abc.Web.Controllers.api
{
    [Route("api/[controller]")]
    public class ItemsController : Controller
    {
        // POST api/values
        [HttpPost]
        public IActionResult Post()
        {
            var requestFormData = Request.Form;                        
            List<Models.Item> lstItems = GetData();
            
            var listItems = ProcessCollection(lstItems, requestFormData);
            
            dynamic response = new
            {
                Data = listItems,
                Draw = requestFormData["draw"],
                RecordsFiltered = lstItems.Count,
                RecordsTotal = lstItems.Count
            };
            return Ok(response);
        }
        
        private List<Models.Item> GetData()
        {
            List<Models.Item> lstItems = new List<Models.Item>()
            {
                new Models.Item() { ItemId =1030,Name ="Bose Mini II", Description ="Wireless and ultra-compact so you can take Bose sound anywhere"  },
                new Models.Item() { ItemId =1031,Name ="Ape Case Envoy Compact - Black (AC520BK)", Description ="Ape Case Envoy Compact Messenger-Style Case for Camera - Black (AC520BK) Removable padded interior in Ape Case signature Hi-Vis yellow protects your equipment"  },
                new Models.Item() { ItemId =1032,Name ="Xbox Wireless Controller - White", Description ="Precision controller compatible with Xbox One, Xbox One S and Windows 10."  },
                new Models.Item() { ItemId =1033,Name ="GoPro HERO5 Black", Description ="Stunning 4K video and 12MP photos in Single, Burst and Time Lapse modes."  },
                new Models.Item() { ItemId =1034,Name ="PNY Elite 240GB USB 3.0 Portable SSD", Description ="PNY Elite 240GB USB 3.0 Portable Solid State Drive (SSD) - (PSD1CS1050-240-FFS)"  },
                new Models.Item() { ItemId =1035,Name ="Quick Charge 2.0 AUKEY 3-Port USB Wall Charger", Description ="Quick Charge 2.0 - Charge compatible devices up to 75% faster than conventional charging"  },
                new Models.Item() { ItemId =1036,Name ="Bose SoundLink Color Bluetooth speaker II - Soft black", Description ="Innovative Bose technology packs bold sound into a small, water-resistant speaker"  },
                new Models.Item() { ItemId = 1010,Name ="RayBan 12300", Description ="Polarized sunglasses"  },
                new Models.Item() { ItemId =1011,Name ="HDMI Cable", Description ="Amzon Basic hdmi cable 3 feet"  },
                new Models.Item() { ItemId =1020,Name ="Anket Portable Charger 500", Description =@"PowerCore Slim 5000
The Slimline Portable Charger

From ANKER, America's Leading USB Charging Brand
• Faster and safer charging with our advanced technology
• 20 million+ happy users and counting"  },
                new Models.Item() { ItemId =1021,Name ="Zippo lighter", Description ="Zippo pocket lighter, black matte"  }

            };

            return lstItems;
        }

        private PropertyInfo getProperty(string name)
        {
            var properties = typeof(Models.Item).GetProperties();
            PropertyInfo prop = null;
            foreach (var item in properties)
            {
                if (item.Name.ToLower().Equals(name.ToLower()))
                {
                    prop = item;
                    break;
                }
            }
            return prop;
        }

        private List<Models.Item> ProcessCollection(List<Models.Item> lstElements, IFormCollection requestFormData)
        {
            var skip = Convert.ToInt32(requestFormData["start"].ToString());
            var pageSize = Convert.ToInt32(requestFormData["length"].ToString());
            Microsoft.Extensions.Primitives.StringValues tempOrder = new[] { "" };

            if (requestFormData.TryGetValue("order[0][column]", out tempOrder))
            {
                var columnIndex = requestFormData["order[0][column]"].ToString();
                var sortDirection = requestFormData["order[0][dir]"].ToString();
                tempOrder = new[] { "" };
                if (requestFormData.TryGetValue($"columns[{columnIndex}][data]", out tempOrder))
                {
                    var columName = requestFormData[$"columns[{columnIndex}][data]"].ToString();

                    if (pageSize > 0)
                    {
                        var prop = getProperty(columName);
                        if (sortDirection == "asc")
                        {
                            return lstElements.OrderBy(prop.GetValue).Skip(skip).Take(pageSize).ToList();
                        }
                        else
                            return lstElements.OrderByDescending(prop.GetValue).Skip(skip).Take(pageSize).ToList();
                    }
                    else
                        return lstElements;
                }
            }
            return null;
        }

    }
}
