package server.client

import org.junit.Test
import java.util.*

class Sort {

    fun selectSort(A: IntArray){
        var k: Int
        for(i in A.indices){
            k = i
            for(j in i+1 until A.size) {
                if (A[k] > A[j])
                    k = j
            }
            A[i] = A[k].also { A[k] = A[i] }
        }
    }


    fun testIsSorted(A: IntArray): Boolean{
        for(i in 1 until A.size){
            if(A[i-1] > A[i])
                return false
        }
        return true
    }

    @Test
    fun testSelectSort(){
        val A = intArrayOf(1, 3, 4, 9, 2, 5, 0, 10, 30, 25)
        selectSort(A)
        assert(testIsSorted(A))
    }

    @Test
    fun testInsertSort(){
        val A = intArrayOf(1, 3, 4, 9, 2, 5, 0, 10, 30, 25)
        insertSort(A)
        assert(testIsSorted(A))
    }

    private fun insertSort(A: IntArray) {
        for(i in 1 until A.size){
            for(j in i downTo  1){
                if(A[j-1] > A[j])
                    A[j-1] = A[j].also { A[j]=A[j-1] }
                else
                    break
            }
        }
    }

    @Test
    fun testBubbleSort(){
        val A = intArrayOf(1, 3, 4, 9, 2, 5, 0, 10, 30, 25)
        bubbleSort(A)
        assert(testIsSorted(A))
        bubbleSort(A)
    }


    private fun bubbleSort(A: IntArray) {
        for(i in A.indices){
            var hasSort = true
            for(j in 1 until A.size-i){
                if(A[j-1] > A[j]) {
                    A[j] = A[j - 1].also { A[j - 1] = A[j] }
                    hasSort = false
                }
            }
            if(hasSort)
                break
        }
    }


    @Test
    fun testMergeSort(){
        val A = intArrayOf(1, 3, 4, 9, 2, 5, 0, 10, 30, 25)
        val sortedA = mergeSort(A)
        assert(testIsSorted(sortedA) && sortedA.size == A.size)
    }

    private fun mergeSort(A: IntArray): IntArray {
        return conquer(A, 0, A.size)
    }

    /**
     * does not contain end
     */
    private fun conquer(A: IntArray, start: Int, end: Int): IntArray {
        if ( end-start <= 1)
            return intArrayOf(A[start])
        else{
            val mid = (end + start) / 2
            val A1 = conquer(A, start, mid)
            val A2 = conquer(A, mid, end)
            return merge2Array(A1, A2)
        }
    }

    private fun merge2Array(A1: IntArray, A2: IntArray): IntArray {
        val res = IntArray(A1.size + A2.size)
        var i = 0
        var i1 = 0
        var i2 = 0
        while (i<res.size){
            if(i1 >= A1.size){ // its A2 time
                while (i<res.size){
                    res[i] = A2[i2];
                    i++
                    i2++
                }
                break
            }
            if(i2 >= A2.size){
                while (i<res.size){
                    res[i] = A1[i1];
                    i++
                    i1++
                }
                break
            }
            if(A1[i1] < A2[i2]){
                res[i] = A1[i1]
                i++
                i1++
            }else{
                res[i] = A2[i2]
                i++
                i2++
            }
        }
        return res
    }


    @Test
    fun testSwap(){
        var a = 1;
        var b = 2;
        a = b.also { b = a }
        assert(a==2)
        assert(b==1)
    }


    internal inner class Solution {

        fun shortestSubarray(A: IntArray, K: Int): Int {
            var res = A.size + 1
            var localSum: Int
            var len: Int
            var start: Int
            val queue = LinkedList<Int>()
            queue.add(0)
            while (!queue.isEmpty()) {
                start = queue.poll()
                localSum = 0
                len = 0
                while (start < A.size && A[start] < 0) {
                    start++
                }
                for (m in start until A.size) {
                    localSum += A[m]
                    len++
                    if (localSum >= K) {
                        res = Math.min(len, res)
                        break
                    }
                }
                if (start + 1 < A.size)
                    queue.add(start + 1)

            }
            return if (res == A.size + 1) -1 else res
        }
    }

    @Test
    fun test0() {
        val s = Solution()
        val A = intArrayOf(1)
        val k = 1
        assert(s.shortestSubarray(A, k) == 1)
    }

    @Test
    fun test1() {
        val s = Solution()
        val A = intArrayOf(1, 2)
        val k = 4
        assert(s.shortestSubarray(A, k) == -1)
    }

    @Test
    fun test2() {
        val s = Solution()
        val A = intArrayOf(2, -1, 2)
        val k = 3
        assert(s.shortestSubarray(A, k) == 3)
    }

    @Test
    fun test3() {
        val s = Solution()
        val A = intArrayOf(48, 99, 37, 4, -31)
        val k = 140
        assert(s.shortestSubarray(A, k) == 2)
    }

    @Test
    fun test4() {
        val s = Solution()
        val A = intArrayOf(84, -37, 32, 40, 95)
        val k = 167
        assert(s.shortestSubarray(A, k) == 3)
    }

    @Test
    fun test5() {
        val s = Solution()
        val A = intArrayOf(44, -25, 75, -50, -38, -42, -32, -6, -40, -47)
        val k = 19
        assert(s.shortestSubarray(A, k) == 1)
    }

    @Test
    fun test6() {
        val s = Solution()
        val A = intArrayOf(-28, 81, -20, 28, -29)
        val k = 89
        assert(s.shortestSubarray(A, k) == 3)
    }


    @Test
    fun test7() {
        val s = Solution()
        val A = intArrayOf(-40, -25, 32, -19, 61, 45, 50, 83, 79, 74, -11, 62, 23, 49, 47, 21, 94, 24, -19, 90)
        val k = 236
        assert(s.shortestSubarray(A, k) == 3)
    }

    @Test
    fun test8() {
        val s = Solution()
        val A = intArrayOf(-34, 37, 51, 3, -12, -50, 51, 100, -47, 99, 34, 14, -13, 89, 31, -14, -44, 23, -38, 6)
        val k = 151
        assert(s.shortestSubarray(A, k) == 2)
    }


}
