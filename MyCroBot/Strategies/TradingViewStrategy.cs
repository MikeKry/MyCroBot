using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyCroBot.Strategies
{
    /// <summary>
    /// Input:
    /// - instrument
    /// - start price
    /// - take profit percents
    /// - stop loss percents
    /// - stop loss gamble percents
    /// - buy step size
    /// 
    /// Algo:
    /// - buys 30% instantly
    /// - in case there is movement UP >= buy step size, buys another 30%
    /// - if profit target is reached, sells and waits for drop or another increase >= buy step size
    /// - if price falls down, waits for the bottom line, until at least 3/4 of buy step size UP trend is available, then places order
    /// - if price is <= stop loss percents, tries to sell a bit above last buy order (stop loss gamble percents)
    /// </summary>
    internal class TradingViewStrategy
    {
    }
}
