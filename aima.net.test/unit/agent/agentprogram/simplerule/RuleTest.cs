using Microsoft.VisualStudio.TestTools.UnitTesting;
using aima.net.agent.api;
using aima.net.agent;
using aima.net.agent.agentprogram.simplerule;

namespace aima.net.test.unit.agent.agentprogram.simplerule
{
    [TestClass]
    public class RuleTest
    { 
        private static readonly IAction ACTION_INITIATE_BRAKING = new aima.net.agent.DynamicAction("initiate-braking");
        private static readonly IAction ACTION_EMERGENCY_BRAKING = new aima.net.agent.DynamicAction("emergency-braking");
        //
        private const string ATTRIBUTE_CAR_IN_FRONT_IS_BRAKING = "car-in-front-is-braking";
        private const string ATTRIBUTE_CAR_IN_FRONT_IS_INDICATING = "car-in-front-is-indicating";
        private const string ATTRIBUTE_CAR_IN_FRONT_TIRES_SMOKING = "car-in-front-tires-smoking";

        [TestMethod]
        public void testEQUALRule()
        {
            Rule r = new Rule(new EQUALCondition(ATTRIBUTE_CAR_IN_FRONT_IS_BRAKING, true), ACTION_INITIATE_BRAKING);

            Assert.AreEqual(ACTION_INITIATE_BRAKING, r.getAction());

            Assert.AreEqual("if car-in-front-is-braking==True then Action[name==initiate-braking].",
                    r.ToString());

            Assert.AreEqual(true, r.evaluate(new DynamicPercept(
                    ATTRIBUTE_CAR_IN_FRONT_IS_BRAKING, true)));

            Assert.AreEqual(false, r.evaluate(new DynamicPercept(
                    ATTRIBUTE_CAR_IN_FRONT_IS_BRAKING, false)));

            Assert.AreEqual(false, r.evaluate(new DynamicPercept(
                    ATTRIBUTE_CAR_IN_FRONT_IS_INDICATING, true)));
        }

        [TestMethod]
        public void testNOTRule()
        {
            Rule r = new Rule(new NOTCondition(new EQUALCondition(
                    ATTRIBUTE_CAR_IN_FRONT_IS_BRAKING, true)),
                    ACTION_INITIATE_BRAKING);

            Assert.AreEqual(ACTION_INITIATE_BRAKING, r.getAction());

            Assert.AreEqual(
                    "if ![car-in-front-is-braking==True] then Action[name==initiate-braking].",
                    r.ToString());

            Assert.AreEqual(false, r.evaluate(new DynamicPercept(
                    ATTRIBUTE_CAR_IN_FRONT_IS_BRAKING, true)));

            Assert.AreEqual(true, r.evaluate(new DynamicPercept(
                    ATTRIBUTE_CAR_IN_FRONT_IS_BRAKING, false)));

            Assert.AreEqual(true, r.evaluate(new DynamicPercept(
                    ATTRIBUTE_CAR_IN_FRONT_IS_INDICATING, true)));
        }

        [TestMethod]
        public void testANDRule()
        {
            Rule r = new Rule(new ANDCondition(new EQUALCondition(
                    ATTRIBUTE_CAR_IN_FRONT_IS_BRAKING, true), new EQUALCondition(
                    ATTRIBUTE_CAR_IN_FRONT_TIRES_SMOKING, true)),
                    ACTION_EMERGENCY_BRAKING);

            Assert.AreEqual(ACTION_EMERGENCY_BRAKING, r.getAction());

            Assert.AreEqual(
                    "if [car-in-front-is-braking==True && car-in-front-tires-smoking==True] then Action[name==emergency-braking].",
                    r.ToString());

            Assert.AreEqual(false, r.evaluate(new DynamicPercept(
                    ATTRIBUTE_CAR_IN_FRONT_IS_BRAKING, true)));

            Assert.AreEqual(false, r.evaluate(new DynamicPercept(
                    ATTRIBUTE_CAR_IN_FRONT_TIRES_SMOKING, true)));

            Assert.AreEqual(true, r.evaluate(new DynamicPercept(
                    ATTRIBUTE_CAR_IN_FRONT_IS_BRAKING, true,
                    ATTRIBUTE_CAR_IN_FRONT_TIRES_SMOKING, true)));

            Assert.AreEqual(false, r.evaluate(new DynamicPercept(
                    ATTRIBUTE_CAR_IN_FRONT_IS_BRAKING, false,
                    ATTRIBUTE_CAR_IN_FRONT_TIRES_SMOKING, true)));

            Assert.AreEqual(false, r.evaluate(new DynamicPercept(
                    ATTRIBUTE_CAR_IN_FRONT_IS_BRAKING, true,
                    ATTRIBUTE_CAR_IN_FRONT_TIRES_SMOKING, false)));
        }

        [TestMethod]
        public void testORRule()
        {
            Rule r = new Rule(new ORCondition(new EQUALCondition(
                    ATTRIBUTE_CAR_IN_FRONT_IS_BRAKING, true), new EQUALCondition(
                    ATTRIBUTE_CAR_IN_FRONT_TIRES_SMOKING, true)),
                    ACTION_EMERGENCY_BRAKING);

            Assert.AreEqual(ACTION_EMERGENCY_BRAKING, r.getAction());

            Assert.AreEqual(
                    "if [car-in-front-is-braking==True || car-in-front-tires-smoking==True] then Action[name==emergency-braking].",
                    r.ToString());

            Assert.AreEqual(true, r.evaluate(new DynamicPercept(
                    ATTRIBUTE_CAR_IN_FRONT_IS_BRAKING, true)));

            Assert.AreEqual(true, r.evaluate(new DynamicPercept(
                    ATTRIBUTE_CAR_IN_FRONT_TIRES_SMOKING, true)));

            Assert.AreEqual(true, r.evaluate(new DynamicPercept(
                    ATTRIBUTE_CAR_IN_FRONT_IS_BRAKING, true,
                    ATTRIBUTE_CAR_IN_FRONT_TIRES_SMOKING, true)));

            Assert.AreEqual(true, r.evaluate(new DynamicPercept(
                    ATTRIBUTE_CAR_IN_FRONT_IS_BRAKING, false,
                    ATTRIBUTE_CAR_IN_FRONT_TIRES_SMOKING, true)));

            Assert.AreEqual(true, r.evaluate(new DynamicPercept(
                    ATTRIBUTE_CAR_IN_FRONT_IS_BRAKING, true,
                    ATTRIBUTE_CAR_IN_FRONT_TIRES_SMOKING, false)));

            Assert.AreEqual(false, r.evaluate(new DynamicPercept(
                    ATTRIBUTE_CAR_IN_FRONT_IS_BRAKING, false,
                    ATTRIBUTE_CAR_IN_FRONT_TIRES_SMOKING, false)));
        }
    }
}
