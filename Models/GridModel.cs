using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Linq;

namespace TestMVC.Models
{
    public class GridModel
    {
        public List<UserView> GetPlayers(int? page, int? limit, string sortBy, string direction, string searchString, out int total)
        {
            var doc = XDocument.Load(HttpContext.Current.Server.MapPath("~/App_Data/Players.xml"));
            total = doc.Descendants("Player").Count();
            var records = (from p in doc.Descendants("Player")
                           select new UserView
                           {
                               ID = int.Parse(p.Element("ID").Value),
                               Name = p.Element("Name").Value,
                               PlaceOfBirth = p.Element("PlaceOfBirth").Value,
                               DateOfBirth = p.Element("DateOfBirth").Value
                           }).AsQueryable();

            if (!string.IsNullOrWhiteSpace(searchString))
            {
                records = records.Where(p => p.Name.Contains(searchString) || p.PlaceOfBirth.Contains(searchString));
            }

            if (!string.IsNullOrEmpty(sortBy) && !string.IsNullOrEmpty(direction))
            {
                if (direction.Trim().ToLower() == "asc")
                {
                    records = SortHelper.OrderBy(records, sortBy);
                }
                else
                {
                    records = SortHelper.OrderByDescending(records, sortBy);
                }
            }

            if (page.HasValue && limit.HasValue)
            {
                int start = (page.Value - 1) * limit.Value;
                records = records.Skip(start).Take(limit.Value);
            }

            return records.ToList();
        }

        public void Save(UserView user)
        {
            string filePath = HttpContext.Current.Server.MapPath("~/App_Data/Players.xml");
            var doc = XDocument.Load(filePath);
            if (user.ID > 0)
            {
                XElement xplayer = (from p in doc.Descendants("Player")
                                    where int.Parse(p.Element("ID").Value) == user.ID
                                    select p).FirstOrDefault();
                if (xplayer != null)
                {
                    xplayer.Element("Name").Value = user.Name;
                    xplayer.Element("PlaceOfBirth").Value = user.PlaceOfBirth;
                    xplayer.Element("DateOfBirth").Value = user.DateOfBirth;
                }
            }
            else
            {
                int? maxId = (from p in doc.Descendants("Player") select int.Parse(p.Element("ID").Value)).OrderByDescending(p => p).FirstOrDefault();
                int newId = maxId.HasValue ? (maxId.Value + 1) : 1;
                XElement node = new XElement("Player");
                node.Add(new XElement("ID", newId));
                node.Add(new XElement("Name", user.Name));
                node.Add(new XElement("PlaceOfBirth", user.PlaceOfBirth));
                node.Add(new XElement("DateOfBirth", user.DateOfBirth));
                doc.Root.Add(node);
            }
            doc.Save(filePath);
        }

        public void Remove(int id)
        {
            string filePath = HttpContext.Current.Server.MapPath("~/App_Data/Players.xml");
            var doc = XDocument.Load(filePath);
            XElement xplayer = (from p in doc.Descendants("Player")
                                where int.Parse(p.Element("ID").Value) == id
                                select p).FirstOrDefault();
            if (xplayer != null)
            {
                xplayer.Remove();
                doc.Save(filePath);
            }
        }
    }
}