//========================================
//  Arduino Pulse 
//    24/11/2020
//    ThibCott JuliDeva
//    Thibaut Cotture
//    EPTM - HVS (Sion)
//    C
//    code de la arduino
//----------------------------------------
// purpose:
// 
//========================================
#include <Wire.h>
#include <LiquidCrystal.h> 

// Instanciation of the LCD object
// pourquoi 12,11,5,4,3,2 => noms des pins suffit
LiquidCrystal lcd(12, 11, 5, 4, 3, 2);

//initialisation des variables
int period = 20; // What is it? and why 20 ?

/*
 * Basic function for Arduino
 * Initialisation of the board
 * --------------------------
 * What is initialized:
 * - LCD :
 *  Basic text that is forever displayed
 * - Serial communication
 */
void setup(void)
{
   // Serial init
   Serial.begin(9600);

   // LCD init 16 caractères pour une ligne, 2 lignes
   lcd.begin(16,2);
  
   //effacer ce qui est ecrit sur le LCD
   lcd.clear();
   
   //affichage sur le LCD du texte static
   lcd.setCursor(0,0);
   lcd.print("Arduino Pulse "); 
   lcd.setCursor(0,1);  
   lcd.print("BPM : ");
}

/*
 * Basic function for Arduino
 * Main code of the board -- infinite loop
 * --------------------------
 * What is done here?
 */
void loop(void)
{
   // what is done here? --retrieve AD value from the sensor
   int ADVal=analogRead(A0);

   // Calcul the hearthbeat why this calcul?
   int hearthbeat = ADVal/10 - 20;
   
   //affichage sur le LCD
   lcd.setCursor(5,1); //erase the old data
   lcd.print("      ");
   lcd.setCursor(5,1); //display the actual data
   lcd.print(hearthbeat);

   //affichage sur l Aplication
   Serial.println(hearthbeat);
   
   // why *10 ?
   delay(period*10);
  
}