using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpPickUp : Collectable {

    public enum PowerUpType { Speed, Jump, Health, Immortal, All};
    [SerializeField] PowerUpType powerUpType = PowerUpType.Health;
    [SerializeField] GameObject FootSteps;

    public override void Pickup() {
        switch (powerUpType) {
            case PowerUpType.Speed:
                i.speedBoost.count++;
                if (!i.speedBoost.hasSeen) {
                    DisplayInformation();
                    i.speedBoost.hasSeen = true;
                    FootSteps.SetActive(false);
                }
                break;

            case PowerUpType.Jump:
                i.jumpBoost.count++;
                if (!i.jumpBoost.hasSeen) {
                    DisplayInformation();
                    i.jumpBoost.hasSeen = true;
                }
                break;

            case PowerUpType.Health:
                i.healthBoost.count++;
                if (!i.healthBoost.hasSeen) {
                    DisplayInformation();
                    i.healthBoost.hasSeen = true;
                }
                break;

            case PowerUpType.Immortal:
                i.immortalBoost.count++;
                if (!i.immortalBoost.hasSeen) {
                    DisplayInformation();
                    i.immortalBoost.hasSeen = true;
                }
                break;

            case PowerUpType.All:
                i.superBoost.count++;
                if (!i.superBoost.hasSeen) {
                    DisplayInformation();
                    i.superBoost.hasSeen = true;
                }
                break;
        }
        i.UpdateUI();
    }
}
