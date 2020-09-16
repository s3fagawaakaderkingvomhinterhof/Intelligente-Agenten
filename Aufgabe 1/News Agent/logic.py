

# Newsagent
# functions for web-requests
from bs4 import BeautifulSoup
import requests


def get_departments():
    url = 'https://www.sueddeutsche.de/'
    response = requests.get(url)
    text = response.text
    soup = BeautifulSoup(text, features="html.parser")
    departments = soup.find('ul', class_='departments').findAll('li')
    categories = []
    for department in departments:
        for ul in department.findAll('ul'):
            ul.decompose()
        name = department.find('a')
        a = department.find('a')
        if name is not None:
            categories.append({'name':name.get('data-title'), 'url':a.attrs.get('href')})
    return categories


def get_news_of(department_url):
    response = requests.get(department_url)
    text = response.text
    soup = BeautifulSoup(text, features="html.parser")
    articles = soup.findAll('a')
    news_teasers = []
    for article in articles:
        article_heading = article.find('h3')
        article_text = article.find('p')
        if article_text is not None and article_heading is not None:
            news_teasers.append({'title': article_heading.string, 'text': article_text.string})
    return news_teasers


def string_builder_of(news_teasers):
    str = ''
    size = len(news_teasers)
    for i in range(0,size):
        str += news_teasers[i].get('title')
        str += '\n'
        str += news_teasers[i].get('text')
        str += '\n'
    return str


## for testing
# print(get_news_of('https://www.sueddeutsche.de/thema/Coronavirus'))


# def get_news_dummy(url):
#    return 'news'#get_news_of('https://www.zeit.de/thema/coronavirus')
