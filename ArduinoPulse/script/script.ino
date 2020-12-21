/*
This example will show you how to use the KY-039 heart bear sensor.
Its a simple basic heart beat monitor with a LCD1602A. In this example i did not use a I2C for those who dont have it.
 */
#include <Wire.h>
#include <LiquidCrystal.h> 

LiquidCrystal lcd(12, 11, 5, 4, 3, 2);

   double alpha = 0.75;
   int period = 20;
   double refresh = 0.0;
void setup(void)
{
   pinMode(A0,INPUT);
   lcd.begin(16,2);
  
   
   lcd.clear();
   //lcd.setCursor(0,0);
   Serial.begin(9600);
   lcd.setCursor(0,0);
   lcd.print("Arduino Pulse "); 
   lcd.setCursor(0,1);  
   
   lcd.print("BPM : ");
}

void loop(void)
{
   static double oldValue=0;
   static double oldrefresh=0;
   int beat=analogRead(A0);
   double value=alpha*oldValue+(0-alpha)*beat;
   refresh=value-oldValue;
   
   //affichage
   lcd.setCursor(5,1);
   lcd.print((beat/10)-20);
   Serial.println((beat/10)-20);
   oldValue=value;
   oldrefresh=refresh;
   delay(period*10);
  
}
