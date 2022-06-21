using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ArduinoValues
{
    private static float desiredMaxValueMovement = 1f;
    private static float desiredMaxValueSpeed = 2.5f;

    public static float xMovement;
    public static float yMovement;

    public static void GetvaluePotXMovement(float potValue)
    {
        //float tempPotValue = (potValue - GlobalPotValues.horizontalValues.minValue) - (GlobalPotValues.horizontalValues.maxValue - GlobalPotValues.horizontalValues.minValue);
        xMovement = -GetCalibrateValue(GlobalPotValues.horizontalValues, potValue);
    }

    public static void GetvaluePotYMovement(float potValue)
    {
        yMovement = GetCalibrateValue(GlobalPotValues.verticalValues, potValue);
    }

    public static float GetValuePotSpeed(float potValue)
    {
        float speedPercentage = (potValue - GlobalPotValues.speedValues.minValue) / (GlobalPotValues.speedValues.maxValue - GlobalPotValues.speedValues.minValue);
        if (speedPercentage <= 0)
        {
            speedPercentage = 0;
        }
        return desiredMaxValueSpeed * (speedPercentage + 0.25f);
    }

    private static float GetCalibrateValue(DifferentPotValues differentPotValues, float potValue)
    {
        float i = 0;

        float tempPotValue = potValue;

        if (tempPotValue > differentPotValues.turnoverValue)
        {
            tempPotValue -= differentPotValues.turnoverValue;
            float tempMaxValue = differentPotValues.maxValue - differentPotValues.turnoverValue;
            i = tempPotValue / tempMaxValue;
        }
        else if (tempPotValue < differentPotValues.turnoverValue)
        {
            tempPotValue -= differentPotValues.minValue;
            tempPotValue = differentPotValues.turnoverValue - tempPotValue;
            float tempTurnoverValue = differentPotValues.turnoverValue - differentPotValues.minValue;
            i = tempPotValue / tempTurnoverValue * -1f;
        }
        else if (tempPotValue == differentPotValues.turnoverValue)
        {
            i = 0;
        }
        return i;
    }

    public static void CheckDirectionalSpeed()
    {
        float maxAngleX = AngleByXCoordinate(xMovement);
        float maxYCoordinateByXAngle = MaxYCoordinate(maxAngleX);

        float maxAngleY = AngleByYCoordinate(yMovement);
        float maxXCoordinateByYAngle = MaxXCoordinate(maxAngleY);

        if (yMovement < maxYCoordinateByXAngle && yMovement > -maxYCoordinateByXAngle)
        {
            return;
        }
        if (xMovement < maxXCoordinateByYAngle && xMovement > -maxXCoordinateByYAngle)
        {
            return;
        }

        float averageAngle = (maxAngleX + maxAngleY) / 2;
        float newXCoordinate = MaxXCoordinate(averageAngle);
        float newYCoordinate = MaxYCoordinate(averageAngle);
        if (newXCoordinate < 0)
        {
            newXCoordinate *= -1;
        }
        if (newYCoordinate < 0)
        {
            newYCoordinate *= -1;
        }

        xMovement *= newXCoordinate;
        yMovement *= newYCoordinate;
    }

    private static float MaxXCoordinate(float angle)
    {
        return Mathf.Cos(angle);
    }

    private static float AngleByXCoordinate(float xCoordinate)
    {
        return Mathf.Round(Mathf.Acos(xCoordinate));
    }

    private static float MaxYCoordinate(float angle)
    {
        return Mathf.Sin(angle);
    }

    private static float AngleByYCoordinate(float yCoordinate)
    {
        return Mathf.Round(Mathf.Asin(yCoordinate));
    }
}
