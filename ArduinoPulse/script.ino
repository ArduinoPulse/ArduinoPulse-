//Projet Arduino Pulse 
//ThibCott JulieDeva
//M306 EPTM

#include <Wire.h>
#include <LiquidCrystal.h>
 
//declaration des pin
//LiquidCrystal lcd(12, 11, 5, 4, 3, 2);
LiquidCrystal lcd(2,6,10,11,12,13);

//inisialisation des variables
double alpha = 0.75;
int period = 20;
double refresh = 0.0;
   
void setup(void)
{
   pinMode(A0,INPUT);
   lcd.begin(16,2);
   lcd.clear();
   lcd.setCursor(0,0);
   Serial.begin(9600);
   lcd.print("Arduino Pulse"); 
}

void loop(void)
{

   static double oldValue=0;
   static double oldrefresh=0;
 
   int beat=analogRead(A0);
  
   double value=alpha*oldValue+(0-alpha)*beat;
   refresh=value-oldValue;  

   lcd.setCursor(0,1);  
   lcd.print("BPM : ");
   lcd.print((beat/10)-20);
   Serial.println((beat/10)-20);
   oldValue=value;
   oldrefresh=refresh;
  
   delay(period*10);
}
