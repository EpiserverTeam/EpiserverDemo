using System;
using System.Collections.Generic;
using EPiServer.Forms.Core.Data;
using EPiServer.Forms.Core;
using EPiServer.Forms.Core.Models;
using System.Linq;
using EPiServer.ServiceLocation;
using EPiServer;
using EPiServer.Forms.Configuration;
using EPiServer.Framework;
using EPiServer.Framework.Initialization;
using EPiServer.Core;
using EPiServer.Forms.Implementation.Elements;
using EpiserverDemo.Models.Blocks;
namespace EpiserverDemo.Store

{

    public class CommentStore
    {
       
        public static List<Submission> GetComment()
        {
            Injected<IFormRepository> _formRepository = new Injected<IFormRepository>();
            List<Submission> submittedData = new List<Submission>();
            FormDataRepository _formDataRepository = new FormDataRepository();

        
            FormContainerBlock _formContainerBlock = new FormContainerBlock();
            
            List<Submission> submittedDataBlock = new List<Submission>();

            var formsInfo = _formRepository.Service.GetFormsInfo(null);

            
            foreach (var info in formsInfo)
            {
                submittedData = _formDataRepository.GetSubmissionData(
                                    new FormIdentity(info.FormGuid, "en"),
                                    DateTime.Now.AddDays(-100),
                                    DateTime.Now).ToList();
            }

            return submittedData;
          
        }

       
    }
}