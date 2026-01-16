using System;
using System.Collections.Generic;
using System.Text;

namespace Roslan.Katapo.Interfaces;

public interface IPlug {



    /// <summary>
    /// Sets the power state of the plug.
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public Task SetPowerStateAsync(bool value);



    /// <summary>
    /// Toggles the power state of the plug.
    /// </summary>
    /// <returns></returns>
    public Task ToggleAsync();

}