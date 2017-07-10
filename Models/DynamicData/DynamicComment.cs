using EPiServer.Data.Dynamic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using EPiServer.Data;
using EPiServer.Core;

namespace EpiTest.Models.DynamicData
{
    [EPiServerDataStore(AutomaticallyRemapStore = true)]
    public class DynamicComment : IDynamicData
    {
        public Identity Id { get; set; }
        public int IdComment { get; set; }
        public int PageID { get; set; }
        public String Comment { get; set; }
        public String Email { get; set; }
        public DateTime TimeComment { get; set; }
        public String Status { get; set; }
        public static List<int> ListPage
        {
            get
            {
                List<int> list = new List<int>();
                foreach (DynamicComment key in GetAllComments())
                {
                    int check = 0;
                    if (list == null) list.Add(key.PageID);
                    else
                    {
                        foreach (int i in list)
                        {
                            if (i == key.PageID)
                            {
                                check = 1;
                                break;
                            }
                        }
                        if (check == 0) list.Add(key.PageID);
                    }

                }
                return list;
            }
        }

        Identity IDynamicData.Id { get; set; }

        protected void Initialize()
        {
            this.Id = Identity.NewIdentity(Guid.NewGuid());
            this.Comment = String.Empty;
            this.Status = "block";
        }

        public DynamicComment()
        {
            Initialize();
            this.IdComment = 0;
            this.Email = "";
            this.Comment = "";
            this.TimeComment = DateTime.Now;
            this.Status = "";
            this.PageID = 0;
        }

        public DynamicComment(int PageID, String email, String text)
        {
            this.IdComment = DynamicComment.LastId().IdComment + 1;
            this.PageID = PageID;
            this.Comment = text;
            this.Email = email;
            this.TimeComment = DateTime.Now;
            this.Status = "none";
        }

        public void Save()
        {
            var store = DynamicDataStoreFactory.Instance.CreateStore(typeof(DynamicComment));
            store.Save(this);

            // Check PageID exist in ListPage
            if (ListPage == null)
            {
                ListPage.Add(DynamicComment.LastComment().PageID);
            }
            else
            {
                int testIdExitsInList = 0;
                foreach (int newID in ListPage)
                {
                    if (newID == LastComment().PageID)
                    {
                        testIdExitsInList = 1;
                        break;
                    }
                }

                if (testIdExitsInList == 0) ListPage.Add(LastComment().PageID);
            }
        }

        // get comments in page which have pageID
        public static List<DynamicComment> GetComments(int PageID)
        {

            var store = DynamicDataStoreFactory.Instance.CreateStore(typeof(DynamicComment));

            var comments = store.Items<DynamicComment>().Where(x => x.PageID == PageID);
            //store.DeleteAll();
            if (comments == null) return new List<DynamicComment>();
            else return comments.ToList();
        }

        // get last comment.id in pageID
        public static DynamicComment LastId()
        {

            var store = DynamicDataStoreFactory.Instance.CreateStore(typeof(DynamicComment));
            var comments = store.Items<DynamicComment>();
            
            if (comments.ToList().Count() == 0) return new DynamicComment();
            return comments.ToList().Last();
        }

        // get last Comment

        public static DynamicComment LastComment()
        {
            var store = DynamicDataStoreFactory.Instance.CreateStore(typeof(DynamicComment));
            var comments = store.Items<DynamicComment>();
            return comments.ToList().Last();
        }

        // get all comments
        public static List<DynamicComment> GetAllComments()
        {

            var store = DynamicDataStoreFactory.Instance.CreateStore(typeof(DynamicComment));

            var comments = store.Items<DynamicComment>();

            if (comments == null) return new List<DynamicComment>();

            return comments.ToList();
        }

        public static void Reset()
        {
            var store = DynamicDataStoreFactory.Instance.CreateStore(typeof(DynamicComment));
            //var comments = store.Items<DynamicComment>().Where(x => x.PageID == PageID);
            store.DeleteAll();
        }

        // get list page
        public static List<int> ListPageInData()
        {

            if (ListPage == null) return new List<int>();
            return ListPage.ToList();
        }

        // delete comment 
       
    }
}