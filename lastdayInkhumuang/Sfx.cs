using Microsoft.Xna.Framework.Audio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lastdayInkhumuang
{
    public class Sfx
    {
        public static List<SoundEffect> sfx = new List<SoundEffect>();
        static List<SoundEffectInstance> instance = new List<SoundEffectInstance>();

        public Sfx (Game1 game)
        {
            //sfx
            sfx.Add(game.Content.Load<SoundEffect>("Sfx/clicked")); //0
            sfx.Add(game.Content.Load<SoundEffect>("Sfx/fireballDrop"));//1
            sfx.Add(game.Content.Load<SoundEffect>("Sfx/fireballHit"));//2
            sfx.Add(game.Content.Load<SoundEffect>("Sfx/Hit attack"));//3
            sfx.Add(game.Content.Load<SoundEffect>("Sfx/HittedHorse"));//4
            sfx.Add(game.Content.Load<SoundEffect>("Sfx/Horsecharge"));//5
            sfx.Add(game.Content.Load<SoundEffect>("Sfx/horseRun"));//6
            sfx.Add(game.Content.Load<SoundEffect>("Sfx/hoverButton"));//7
            sfx.Add(game.Content.Load<SoundEffect>("Sfx/jazz-loop-rusted-maid-68244"));//8
            sfx.Add(game.Content.Load<SoundEffect>("Sfx/m_enemy_attack"));//9
            sfx.Add(game.Content.Load<SoundEffect>("Sfx/pheonixAtk"));//10
            sfx.Add(game.Content.Load<SoundEffect>("Sfx/PlayerHurt"));//11
            sfx.Add(game.Content.Load<SoundEffect>("Sfx/RangeEnemyMagic"));//12
            sfx.Add(game.Content.Load<SoundEffect>("Sfx/select"));//13
            sfx.Add(game.Content.Load<SoundEffect>("Sfx/walk"));//14
            sfx.Add(game.Content.Load<SoundEffect>("Sfx/Three"));//15
            sfx.Add(game.Content.Load<SoundEffect>("Sfx/spearHorseHit"));//16
            sfx.Add(game.Content.Load<SoundEffect>("Sfx/phoenixSpear"));//17
            sfx.Add(game.Content.Load<SoundEffect>("Sfx/Miss attack"));//18

            //bg
            sfx.Add(game.Content.Load<SoundEffect>("Bgm/Title"));//19
            sfx.Add(game.Content.Load<SoundEffect>("Bgm/GeeGee"));//20
            sfx.Add(game.Content.Load<SoundEffect>("Bgm/Dullahan"));//21

            //Sfx
            sfx.Add(game.Content.Load<SoundEffect>("Sfx/horseRush"));//22
            sfx.Add(game.Content.Load<SoundEffect>("Sfx/PlayerDash"));//23
            sfx.Add(game.Content.Load<SoundEffect>("Sfx/PlayerSkill"));//24
            sfx.Add(game.Content.Load<SoundEffect>("Sfx/horsedead"));//25

            //Setting
            for (int i = 0; i < sfx.Count; i++)
            {
                instance.Add(sfx[i].CreateInstance());
            }
            for (int i = 19; i < 22; i++)
            {
                instance[i].Volume = 0.2f;
                instance[i].IsLooped = true;
            }
            instance[14].Volume = 1.0f;


            SoundEffect.MasterVolume = 0.6f;
        }

        public static void PlaySfx(int index)
        {
            sfx[index].Play();
        }
        public static void InsPlaySfx(int index)
        {
            instance[index].Play();
        }
        public static void InsStopSfx(int index)
        {
            instance[index].Stop();
        }
    }
}
