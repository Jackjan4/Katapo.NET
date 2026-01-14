using System;
using System.Collections.Generic;
using System.Text;

namespace Roslan.Katapo.Interfaces;

public interface IPlug {


    public Task SetPower(bool value);

    public Task Toggle();

}