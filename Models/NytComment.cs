using EPiServer.Forms.Core.PostSubmissionActor;

namespace EpiTest.Models
{
    public class CustomActor : PostSubmissionActorBase
    {
        public override object Run(object input)
        {
            var submittedData = SubmissionData.Data;

            return submittedData;
        }

    }
}