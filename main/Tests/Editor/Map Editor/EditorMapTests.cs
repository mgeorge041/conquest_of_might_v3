using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests
{
    public class EditorMapTests
    {
        // Creates editor map
        private EditorMap CreateEditorMap() {
            EditorMap editorMap = new EditorMap();
            return editorMap;
        }

        // Test editor map created
        [Test]
        public void EditorMapCreated()
        {
            EditorMap editorMap = CreateEditorMap();
            Assert.IsNotNull(editorMap);
        }

        // Test setting selected paint tile
        [Test]
        public void SelectedTileSetCorrectly() {

        }

        // A UnityTest behaves like a coroutine in Play Mode. In Edit Mode you can use
        // `yield return null;` to skip a frame.
        [UnityTest]
        public IEnumerator MapEditorTestsWithEnumeratorPasses()
        {
            // Use the Assert class to test conditions.
            // Use yield to skip a frame.
            yield return null;
        }
    }
}
