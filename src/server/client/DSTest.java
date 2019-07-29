package server.client;

import org.junit.Test;

public class DSTest {
    @Test
    public void testHeap1() throws Exception {
        int[] nums = {2, -4, 4, 9,-5, 10, 23, -6, 11};
        ArrayHeap arrayHeap = new ArrayHeap(9);
        for(int num:nums){
            arrayHeap.insert(num);
        }
        while (arrayHeap.curr > 0){
            System.out.println(arrayHeap.deleteMin());
        }
    }

    @Test
    public void testHeap2() throws Exception {
        int[] nums = {2, -4, 4, 9,-5, 10, 23, -6, 11};
        ArrayHeap arrayHeap = new ArrayHeap(nums);
        while (arrayHeap.curr > 0){
            System.out.println(arrayHeap.deleteMin());
        }
    }
}
