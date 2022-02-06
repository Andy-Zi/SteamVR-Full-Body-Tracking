using PTSC.Interfaces;
using System.Numerics;


namespace PTSC.Pipeline
{
	//see https://social.msdn.microsoft.com/Forums/en-US/eb647eeb-26ef-45d6-ba73-ac26b8b46925/joint-orientation-smoothing-unity-c?forum=kinectv2sdk
	public class RotationSmoothingContainer
	{
		RotationSmoothing Hip;
		RotationSmoothing Left;
		RotationSmoothing Right;
		public RotationSmoothingContainer(int size = 10)
        {
			Hip = new RotationSmoothing(size);
			Left = new RotationSmoothing(size);
			Right = new RotationSmoothing(size);
		}

		public void Apply(IDriverData driverData)
        {
			driverData.left_foot.setRotation(Left.Apply(driverData.left_foot.getRotation()));
			driverData.right_foot.setRotation(Right.Apply(driverData.right_foot.getRotation()));
			driverData.waist.setRotation(Hip.Apply(driverData.waist.getRotation()));
		}
	}
	
    public class RotationSmoothing
    {
		protected object LockObject = new object(); 
        public readonly int queueSize = 15;
        private Queue<Quaternion> rotations;

		public RotationSmoothing(int size = 15)
        {
            queueSize = size;
            rotations = new Queue<Quaternion>(queueSize);
        }


		private Quaternion SmoothFilter(Quaternion newValue)
		{


			if (rotations.Count == 0)
				return newValue;

			Quaternion median = new Quaternion(0, 0, 0, 0);

			int counter = 0;
			foreach (Quaternion quaternion in rotations)
			{
				
				Quaternion weightedQuaternion = Quaternion.Lerp(newValue, quaternion, counter/(float)queueSize);

				median.X += weightedQuaternion.X;
				median.Y += weightedQuaternion.Y;
				median.Z += weightedQuaternion.Z;
				median.W += weightedQuaternion.W;

				counter++;
			}

			median.X /= rotations.Count;
			median.Y /= rotations.Count;
			median.Z /= rotations.Count;
			median.W /= rotations.Count;

			return Quaternion.Normalize(median);
		}

		public Quaternion Apply(Quaternion quaternion)
        {
            lock (LockObject)
            {
				var result = SmoothFilter(quaternion);
				this.rotations.Enqueue(result);
				if (this.rotations.Count - 1 >= queueSize)
					this.rotations.Dequeue();
				return result;
			}
        }
    }
}
