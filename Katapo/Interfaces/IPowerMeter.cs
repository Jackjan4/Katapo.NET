using System;
using System.Collections.Generic;
using System.Text;

namespace Roslan.Katapo.Interfaces;

public interface IPowerMeter {

    public Task<double> GetCurrentPowerAsync();

}