using ImageFinder.Presentation.EventTriggers;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace ImageFinder.Presentation.Test
{
    [TestFixture]
    class EscapeKeyDownEventTriggerFixture
    {
        EscapeKeyDownEventTrigger _fixture;

        [SetUp]
        public void Setup()
        {
            _fixture = new EscapeKeyDownEventTrigger();
        }

        [Test]
        public void Should_Be_KeyDown_Event()
        {
            Assert.AreEqual(_fixture.EventName, "KeyDown");
        }

    }
}
