﻿using MelonLoader;

namespace BlasII.QualityOfLife;

public class Main : MelonMod
{
    public static QualityOfLife QualityOfLife { get; private set; }

    public override void OnLateInitializeMelon()
    {
        QualityOfLife = new QualityOfLife();
    }
}