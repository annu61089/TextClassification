﻿// Accord Unit Tests
// The Accord.NET Framework
// http://accord-framework.net
//
// Copyright © César Souza, 2009-2017
// cesarsouza at gmail.com
//
//    This library is free software; you can redistribute it and/or
//    modify it under the terms of the GNU Lesser General Public
//    License as published by the Free Software Foundation; either
//    version 2.1 of the License, or (at your option) any later version.
//
//    This library is distributed in the hope that it will be useful,
//    but WITHOUT ANY WARRANTY; without even the implied warranty of
//    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
//    Lesser General Public License for more details.
//
//    You should have received a copy of the GNU Lesser General Public
//    License along with this library; if not, write to the Free Software
//    Foundation, Inc., 51 Franklin St, Fifth Floor, Boston, MA  02110-1301  USA
//

namespace Accord.Tests.MachineLearning
{
    using System;
    using System.IO;
    using System.Runtime.Serialization.Formatters.Binary;
    using Accord.MachineLearning;
    using Accord.Math;
    using NUnit.Framework;
    using Accord.Statistics.Models.Regression;
    using Accord.Statistics.Models.Regression.Fitting;

    [TestFixture]
    public class BagOfWordsTest
    {

        string[][] texts =
        {
            "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Maecenas molestie malesuada nisi et placerat. Curabitur blandit porttitor suscipit. Nunc facilisis ultrices felis, vitae luctus arcu semper in. Fusce ut felis ipsum. Sed faucibus tortor ut felis placerat euismod. Vestibulum pharetra velit et dolor ornare quis malesuada leo aliquam. Aenean lobortis, tortor iaculis vestibulum dictum, tellus nisi vestibulum libero, ultricies pretium nisi ante in neque. Integer et massa lectus. Aenean ut sem quam. Mauris at nisl augue, volutpat tempus nisl. Suspendisse luctus convallis metus, vitae pretium risus pretium vitae. Duis tristique euismod aliquam".Replace(",", "").Replace(".", "").Split(' '),

            "Sed consectetur nisl et diam mattis varius. Aliquam ornare tincidunt arcu eget adipiscing. Etiam quis augue lectus, vel sollicitudin lorem. Fusce lacinia, leo non porttitor adipiscing, mauris purus lobortis ipsum, id scelerisque erat neque eget nunc. Suspendisse potenti. Etiam non urna non libero pulvinar consequat ac vitae turpis. Nam urna eros, laoreet id sagittis eu, posuere in sapien. Phasellus semper convallis faucibus. Nulla fermentum faucibus tellus in rutrum. Maecenas quis risus augue, eu gravida massa.".Replace(",", "").Replace(".", "").Split(' ')
        };

        [Test]
        public void GetFeatureVectorTest()
        {
            BagOfWords target = new BagOfWords(texts);

            string[] text = { "Lorem", "ipsum", "dolor" };

            int[] actual = target.GetFeatureVector(text);

            Assert.IsTrue(actual[0] == 1);
            Assert.IsTrue(actual[1] == 1);
            Assert.IsTrue(actual[2] == 1);

            for (int i = 3; i < actual.Length; i++)
                Assert.IsFalse(actual[i] == 1);
        }

        [Test]
        public void GetFeatureVectorTest2()
        {
            BagOfWords target = new BagOfWords(texts);

            string[] text = { "Lorem", "test", "dolor" };

            int[] actual = target.GetFeatureVector(text);

            Assert.IsTrue(actual[0] == 1);
            Assert.IsTrue(actual[1] == 0);
            Assert.IsTrue(actual[2] == 1);

            for (int i = 3; i < actual.Length; i++)
                Assert.IsFalse(actual[i] == 1);
        }

        [Test]
        public void ComputeTest()
        {
            BagOfWords target = new BagOfWords();

            target.Compute(texts);

            target.MaximumOccurance = Int16.MaxValue;

            string[] text = { "vestibulum", "vestibulum", "vestibulum" };

            int[] actual = target.GetFeatureVector(text);

            int actualIdx = 43;

            Assert.IsTrue(actual[actualIdx] == 3);

            for (int i = 0; i < actual.Length; i++)
            {
                if (i != actualIdx)
                    Assert.IsTrue(actual[i] == 0);
            }
        }

        [Test]
        public void SerializationTest()
        {
            BagOfWords target = new BagOfWords();

            target.Compute(texts);

            int[][] expected = new int[texts.Length][];
            for (int i = 0; i < expected.Length; i++)
                expected[i] = target.GetFeatureVector(texts[i]);

            MemoryStream stream = new MemoryStream();
            BinaryFormatter fmt = new BinaryFormatter();
            fmt.Serialize(stream, target);
            stream.Seek(0, SeekOrigin.Begin);
            target = (BagOfWords)fmt.Deserialize(stream);

            int[][] actual = new int[expected.Length][];
            for (int i = 0; i < actual.Length; i++)
                actual[i] = target.GetFeatureVector(texts[i]);

            Assert.IsTrue(expected.IsEqual(actual));
        }

        [Test]
        public void learn_test()
        {
            #region doc_learn
            // The Bag-Of-Words model can be used to extract finite-length feature 
            // vectors from sequences of arbitrary length, like for example, texts:
            

            string[] texts =
            {
                @"Lorem ipsum dolor sit amet, consectetur adipiscing elit. Maecenas molestie malesuada 
                  nisi et placerat. Curabitur blandit porttitor suscipit. Nunc facilisis ultrices felis,
                  vitae luctus arcu semper in. Fusce ut felis ipsum. Sed faucibus tortor ut felis placerat
                  euismod. Vestibulum pharetra velit et dolor ornare quis malesuada leo aliquam. Aenean 
                  lobortis, tortor iaculis vestibulum dictum, tellus nisi vestibulum libero, ultricies 
                  pretium nisi ante in neque. Integer et massa lectus. Aenean ut sem quam. Mauris at nisl 
                  augue, volutpat tempus nisl. Suspendisse luctus convallis metus, vitae pretium risus 
                  pretium vitae. Duis tristique euismod aliquam",

                @"Sed consectetur nisl et diam mattis varius. Aliquam ornare tincidunt arcu eget adipiscing. 
                  Etiam quis augue lectus, vel sollicitudin lorem. Fusce lacinia, leo non porttitor adipiscing, 
                  mauris purus lobortis ipsum, id scelerisque erat neque eget nunc. Suspendisse potenti. Etiam 
                  non urna non libero pulvinar consequat ac vitae turpis. Nam urna eros, laoreet id sagittis eu,
                  posuere in sapien. Phasellus semper convallis faucibus. Nulla fermentum faucibus tellus in 
                  rutrum. Maecenas quis risus augue, eu gravida massa."
            };

            string[][] words = texts.Tokenize();

            // Create a new BoW with options:
            var codebook = new BagOfWords()
            {
                MaximumOccurance = 1 // the resulting vector will have only 0's and 1's
            };

            // Compute the codebook (note: this would have to be done only for the training set)
            codebook.Learn(words);

            
            // Now, we can use the learned codebook to extract fixed-length
            // representations of the different texts (paragraphs) above:

            // Extract a feature vector from the text 1:
            double[] bow1 = codebook.Transform(words[0]);

            // Extract a feature vector from the text 2:
            double[] bow2 = codebook.Transform(words[1]);

            // we could also have transformed everything at once, i.e.
            // double[][] bow = codebook.Transform(words);


            // Now, since we have finite length representations (both bow1 and bow2 should
            // have the same size), we can pass them to any classifier or machine learning
            // method. For example, we can pass them to a Logistic Regression Classifier to
            // discern between the first and second paragraphs

            // Lets create a Logistic classifier to separate the two paragraphs:
            var learner = new IterativeReweightedLeastSquares<LogisticRegression>()
            {
                Tolerance = 1e-4,  // Let's set some convergence parameters
                Iterations = 100,  // maximum number of iterations to perform
                Regularization = 0
            };

            // Now, we use the learning algorithm to learn the distinction between the two:
            LogisticRegression reg = learner.Learn(new[] { bow1, bow2 }, new[] { false, true});

            // Finally, we can predict using the classifier:
            bool c1 = reg.Decide(bow1); // Should be false
            bool c2 = reg.Decide(bow2); // Should be true
            #endregion

            Assert.AreEqual(bow1.Length, 99);
            Assert.AreEqual(bow2.Length, 99);

            Assert.AreEqual(bow1.Sum(), 67);
            Assert.AreEqual(bow2.Sum(), 63);

            Assert.IsFalse(c1);
            Assert.IsTrue(c2);
        }
    }
}
