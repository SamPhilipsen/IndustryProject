using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ArduinoValues
{
    private static float maxPotValue = 900f;
    private static float desiredMaxValueMovement = 1f;
    private static float desiredMaxValueSpeed = 4f;

    public static float xMovement;
    public static float yMovement;

    public static void GetvaluePotXMovement(float potValue)
    {
        float value = desiredMaxValueMovement / maxPotValue * potValue;
        xMovement = (value - (desiredMaxValueMovement / 2)) * 2;
    }

    public static void GetvaluePotYMovement(float potValue)
    {
        float value = desiredMaxValueMovement / maxPotValue * potValue;
        yMovement = (value - (desiredMaxValueMovement / 2)) * 2;
    }

    public static float GetValuePotSpeed(float potValue)
    {
        return desiredMaxValueSpeed / maxPotValue * potValue + 1;
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
