namespace Scrips.SpecialEffects
{
	public class ReduceArmor : BaseSpecialEffect {

		public float Amount;

		// Use this for initialization
		void Start () {

		}

		// Update is called once per frame
		void Update () {

		}
		public override void ApplySpecialEffect(Scrips.EnemyData.Instances.EnemyInstance target)
		{
			target.ReduceArmor(Amount);
		}
	}
}
