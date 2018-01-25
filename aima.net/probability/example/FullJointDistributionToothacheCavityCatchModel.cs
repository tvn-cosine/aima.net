﻿using aima.net.probability.full;

namespace aima.net.probability.example
{
    public class FullJointDistributionToothacheCavityCatchModel : FullJointDistributionModel
    { 
        public FullJointDistributionToothacheCavityCatchModel()
            : base(new double[] {
				// Toothache = true, Cavity = true, Catch = true
				0.108,
				// Toothache = true, Cavity = true, Catch = false
				0.012,
				// Toothache = true, Cavity = false, Catch = true
				0.016,
				// Toothache = true, Cavity = false, Catch = false
				0.064,
				// Toothache = false, Cavity = true, Catch = true
				0.072,
				// Toothache = false, Cavity = true, Catch = false
				0.008,
				// Toothache = false, Cavity = false, Catch = true
				0.144,
				// Toothache = false, Cavity = false, Catch = false
				0.576 }, ExampleRV.TOOTHACHE_RV, ExampleRV.CAVITY_RV,
                    ExampleRV.CATCH_RV)
        { }
    } 
}
