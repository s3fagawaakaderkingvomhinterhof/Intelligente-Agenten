package com.vogella.maven.quickstart;

import java.net.MalformedURLException;
import java.util.List;
import java.util.concurrent.Delayed;

import org.openqa.selenium.By;
import org.openqa.selenium.Keys;
import org.openqa.selenium.WebDriver;
import org.openqa.selenium.WebElement;
import org.openqa.selenium.chrome.ChromeDriver;
import org.openqa.selenium.chrome.ChromeOptions;
import org.openqa.selenium.support.ui.Wait;

public class App 
{
	
    public static void main( String[] args ) throws MalformedURLException
    {
    	
    	System.out.println("started App");
    	
    	String searchLocation, urlLocation, temperatureLocation;
    	searchLocation = args[0];
    	
    	WebElement search, found;
    	
    	/*
    	 * Chromedriver Version muss mit der des installierten Chromebrowser �berinstimmen.
    	 */
    	System.setProperty("webdriver.chrome.driver", "Z:\\Studium\\intelligente_agenten\\Uebung1\\falko\\driver\\chromedriver_85.exe");
    	
    	WebDriver driver;
    	
    	ChromeOptions chromeOptions = new ChromeOptions();
        chromeOptions.addArguments("--no-sandbox");
        chromeOptions.addArguments("--headless");
        chromeOptions.addArguments("disable-gpu");
        
        driver = new ChromeDriver(chromeOptions);
        //WebDriver driver = new ChromeDriver();
    	
        driver.get("https://www.wetteronline.de/");
        
        System.out.println(driver.getCurrentUrl());
        
        search = driver.findElement(By.id("searchstring"));
        search.sendKeys(searchLocation + Keys.ENTER);
        
        urlLocation = driver.getCurrentUrl();
        driver.get(urlLocation);
        
        found = driver.findElement(By.id("nowcast-card-temperature"));
        temperatureLocation = found.getText();
        
        System.out.println("=============================================");
        
        //System.out.println(temperatureLocation.length());
        System.out.println(temperatureLocation);
        
        driver.quit();
        
    }
}
