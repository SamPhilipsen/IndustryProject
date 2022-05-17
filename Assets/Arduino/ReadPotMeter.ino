int analogPin1 = A1;
int analogPin2 = A2;
int analogPin3 = A3;

String beginToken = "%";
String divider = ":";
String endToken = "#";

int val1, val2, val3;

void setup() {
  Serial.begin(9600);
}

void loop() {
  
  val1 = analogRead(analogPin1);
  val2 = analogRead(analogPin2);
  val3 = analogRead(analogPin3);
  
  sendMessage();
}

void sendMessage()
{
    Serial.println(beginToken + "Speed" + divider + val1 + endToken);
    delay(10);
    Serial.println(beginToken + "Direction" + divider + val2 + endToken);
    delay(10);
    Serial.println(beginToken + "Height" + divider + val3 + endToken);
    delay(1000);
}
