using System;
using System.Collections;
using System.Collections.Generic;
using EPiServer;
using EPiServer.Core;
using EPiServer.DataAccess;
using EpiserverDemo.Models.Pages;
using EPiServer.Security;


namespace EpiserverDemo.Models
{
    public class BaseClass<T> : ContentData, IEnumerable<T>
    {
        //public IEnumerable<T> createPage(IContentRepository contentRepository, string IDofPageParent, string PageName)
        //{
        //    IEnumerable<T> myPage = contentRepository.GetDefault<ContainerPage>(ContentReference.Parse(IDofPageParent));
        //    myPage.Name = PageName;

        //    contentRepository.Save(myPage, SaveAction.Publish, AccessLevel.NoAccess);
        //    return myPage;
        //}
        public IEnumerator<T> GetEnumerator()
        {
            throw new NotImplementedException();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }
    }
}