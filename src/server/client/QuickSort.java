package server.client;

import org.junit.Test;

import java.util.Arrays;


public class QuickSort {

    private void sort(int[] nums) {
        int[] temp = new int[nums.length];
        quickSort(nums, temp, 0, nums.length);
    }

    private void quickSort(int[] nums, int[] temp, int lo, int hi) {
        if(hi-lo <= 1)
            return;
        int p = partition(nums, temp, lo, hi);
        quickSort(nums, temp, lo, p);
        quickSort(nums, temp, p + 1, hi);
    }

    private int partition(int[] nums, int[] temp, int lo, int hi) {
        int p = (int) (Math.random() * (hi - lo - 1)) + lo;
        int pivot = nums[p];
        int L = lo, R = hi-1;
        for (int i = lo; i < hi; i++) {
            if (nums[i] < pivot) {
                temp[L] = nums[i];
                L++;
            } else if (i != p) {
                temp[R] = nums[i];
                R--;
            }
        }
        temp[R] = nums[p];
        if (hi - lo >= 0) System.arraycopy(temp, lo, nums, lo, hi - lo);
        return R;
    }

    @Test
    public void testSort() {
        int[] nums = {1, 3, -20, 24, 45, -2, 12, 19, 30};
        int[] arrbak = nums.clone();
        Arrays.sort(arrbak);
        sort(nums);
        assert Arrays.toString(nums).equals(Arrays.toString(arrbak));
    }


}
