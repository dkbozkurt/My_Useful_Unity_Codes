// Dogukan Kaan Bozkurt
//      github.com/dkbozkurt

using System;
using Unity.Burst;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;
using Unity.Collections;

namespace MultiThreading.JobSystem
{
    /// <summary>
    /// Ref : https://www.youtube.com/watch?v=C56bbgtPr_w&ab_channel=CodeMonkey until 12.27
    /// </summary>
    public class JobSystemTesting : MonoBehaviour
    {
        [SerializeField] private bool _useJobs;

        private void Update()
        {
            if (_useJobs)
            {
                CalculateMSForJob();
            }
            else
            {
                CalculateMS(ReallyToughTask);
            }
        }

        // Represents a tough task like some pathfinding or a really complex calculation
        private void ReallyToughTask()
        {
            float value = 0f;
            for (int i = 0; i < 50000; i++)
            {
                value = math.exp10(math.sqrt(value));
            }
        }

        private void CalculateMS(Action function)
        {
            float startTime = Time.realtimeSinceStartup;
            for (int i = 0; i < 10; i++)
            {
                function?.Invoke();    
            }
            Debug.Log(((Time.realtimeSinceStartup - startTime) * 1000f) + "ms");
        }

        private void CalculateMSForJob()
        {
            float startTime = Time.realtimeSinceStartup;
            
            NativeList<JobHandle> jobHandleList = new NativeList<JobHandle>(Allocator.Temp);
            for (int i = 0; i < 10; i++)
            {
                JobHandle jobHandle = ReallyToughTaskJob();
                jobHandleList.Add(jobHandle);
            }

            JobHandle.CompleteAll(jobHandleList);
            jobHandleList.Dispose();

            Debug.Log(((Time.realtimeSinceStartup - startTime) * 1000f) + "ms");
        }

        private JobHandle ReallyToughTaskJob()
        {
            ReallyToughJob job = new ReallyToughJob();
            return job.Schedule();
        }
    }
    
    /// <summary>
    /// Jobs can not access all the values so if there is a missing one, create a new public variable to assign it.
    /// </summary>
    [BurstCompile]
    public struct ReallyToughJob : IJob
    {
        public void Execute()
        {
            float value = 0f;
            for (int i = 0; i < 50000; i++)
            {
                value = math.exp10(math.sqrt(value));
            }
        }
    }
}