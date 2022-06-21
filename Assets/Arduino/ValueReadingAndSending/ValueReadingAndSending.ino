int speedPin = 12;
int directionPin = A2;
int updownPin = A1;

String beginToken = "%";
String divider = ":";
String endToken = "#";

int val, sensorState, speedVal, directionVal, updownVal, speedAmount;

unsigned long previousMillis = 0;
const long interval = 3000;

void setup() {
  Serial.begin(9600);
}

void loop() {
  directionVal = analogRead(directionPin);
  updownVal = analogRead(updownPin);

  
  CalculateRPM();
  SendMessage();
  unsigned long currentMillis = millis();

  if (currentMillis - previousMillis >= interval) {
    previousMillis = currentMillis;
    SetRPM();
  }
}

void CalculateRPM()
{
  val = digitalRead(speedPin);
       if (val != sensorState) {         
        if (val == LOW) {               
          speedAmount++;               
        }
      }
      sensorState = val;
      Serial.println(speedAmount);
}

void SetRPM()
{
  speedVal = speedAmount;
  speedAmount = 0;
}

void SendMessage()
{
    Serial.println(beginToken + "Speed" + divider + speedVal + endToken);
    Serial.println(beginToken + "Direction" + divider + directionVal + endToken);
    Serial.println(beginToken + "Height" + divider + updownVal + endToken);
    delay(100);
}
