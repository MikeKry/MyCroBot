# MyCroBot
Implementation of Crypto.com Exchange API. Personal automatized trading project with multiple strategies. 

> [!CAUTION]
> Disclaimer! Use this bot at your own risk and do your own tests and due dilligence! This bot is created for education purposes only, using it is not a financial advice, it is not recommended for investment purposes. Use this code at your own risk, you might lose your funds by using this bot. This software is for educational purposes only. Do not risk money which you are afraid to lose. USE THE SOFTWARE AT YOUR OWN RISK. THE AUTHORS AND ALL AFFILIATES ASSUME NO RESPONSIBILITY FOR YOUR TRADING RESULTS.
> It is recommend to have C# coding skills and to test with manual action confirmations / without funds before running this bot.

> [!NOTE]  
> This is a very early stage of development, currently I am using it just on manually supervised mode and it does pretty much nothing on its own. Also needs a good refactor, but right now it is a working prototype.


Settings:
- create some sub-account on crypto.com exchange with limited funds (that you can experiment with)
- create api key with trading permissions
- whitelist your IP for that api key
- enter your api key and secret in App_Data/appsettings.json
- update settings in BotRunner.cs (will be moved and changed a lot..)

Running bot:
- currently supports "basic strategy"
- this checks your balance, checks if your conditions for trade are met and asks you if you want to perform Buy trade
- then it periodically after 5s checks moving averages in range of 5 - 50 minutes and tries to evalute wheteher to buy or sell (very naive, but it is just a prototype yet)
- every action (buy / sell) is manually prompted for your permission


Roadmap:
- finish basic strategy
  - testing, try to achieve some positive yield
- refactor code
  - update api models
  - use strategy / command patterns for building custom strategies
- add indicators and math functions (MA, ...) to predict whether to enter / exit trade
- test environment (without really trading)
- add database for keeping trading data
- build visual / web UI for configuration of strategies and profits
