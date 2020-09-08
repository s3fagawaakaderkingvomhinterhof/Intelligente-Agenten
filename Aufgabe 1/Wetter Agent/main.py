import sys
from bs4 import BeautifulSoup                                   # library for html-parsing
import requests                                                 # library for requests

def getWeatherFor(location):                                    # weather function
    url = 'https://wetter.de/suche.html?search=' + location     # source url
    res = requests.get(url)                                     # save response
    website = res.text                                          # extract html text from response
    soup = BeautifulSoup(website, features="html.parser")       # define var, save site and define parser
    dailyForecasts = soup.findAll('div', class_=['base-box--level-0', 'weather-daybox']) # go through all days
    forecast = []                                               # define structure too save days
    for dailyForecastString in dailyForecasts:
        daySoup = BeautifulSoup(str(dailyForecastString), features="html.parser")   # parse for string
        day = daySoup.find('div', class_='weather-daybox__date__weekday').string    # parse for day
        max = daySoup.find('div', class_='weather-daybox__minMax__max').string      # parse for max temp
        min = daySoup.find('div', class_='weather-daybox__minMax__min').string      # parse fro min temp
        forecastday = {"day": day, "max": max, "min": min}                          # build forecast structure
        forecast.append(forecastday)                                                # put it in list
    print("\t***************************")
    print("\t*** Wetter für " + location)                                                               # print result
    print("\t***************************")
    print("\t*** " + forecast[0].get('day') + " : " +forecast[0].get('max')+"/"+forecast[0].get('min')) # print result
    print("\t***************************")
    print("\t*** " + forecast[1].get('day') + " : " +forecast[1].get('max')+"/"+forecast[1].get('min')) # print result
    print("\t***************************")


getWeatherFor(sys.argv[1])                                      # call of weather function
