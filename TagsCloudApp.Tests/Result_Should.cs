using System;
using System.IO;
using NUnit.Framework;
using FluentAssertions;
using TagsCloudApp.DataInput;

namespace TagsCloudApp.Tests
{
    public class Result_Should
    {
        [Test]
        public void RunThenTry_WhenOk()
        {
            var res = Result.Ok(42)
                .ThenTry((c) => c.ToString(), null);
           
            res.ShouldBeEquivalentTo(Result.Ok("42"));
        }

        [Test]
        public void ChangeException_FromThenTry_WhenFail()
        {
            var res = Result.Ok(42)
                .ThenTry((c) =>
            {
                throw new ArgumentException();
                return "not ok";
            } ,"incorrect data");
            
            res.Error.ShouldBeEquivalentTo("incorrect data");
        }

        [Test]
        public void NotChangeException_FromThenTry_WhenFailWasBefore()
        {
            var res = Result.Ok(42)
                .ThenTry((c) =>
                {
                    throw new ArgumentException();
                    return "not ok";
                }, "incorrect data")
                .ThenTry(c => {
                    throw new ArgumentException();
                    return "not ok";
                },"new ecxeption");

            res.Error.ShouldBeEquivalentTo("incorrect data");
        }

       
    }
}