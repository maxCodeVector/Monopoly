package server.client;

import org.junit.Assert;
import org.junit.Test;

import java.util.Stack;

public class LeetCode {

    class Solution {

        class State{
            int pos;
            int nextMatched;

            State(int pos, int nextMatched) {
                this.pos = pos;
                this.nextMatched = nextMatched;
            }

            public String toString(){
                return String.format("pos: %d, next: %d", pos, nextMatched);
            }
        }


        public boolean isMatch(String s, String p) {
            if(s.length()==0){
                int i=0;
                while (i<p.length()){
                    if(p.charAt(i)!='*'){
                        return false;
                    }
                    i++;
                }
                return true;
            }
            if(p.length()==0){
                return false;
            }
            int currMatched = 0;
            Stack<State> stateStack = new Stack<State>();
            int i=0;
            while (true){
                if(i >= p.length()){
                    if(currMatched<s.length() &&!stateStack.empty()) {
                        while (!stateStack.empty()) {
                            State pre = stateStack.pop();
                            State newS = back(s, p, pre);
                            if (newS != null) {
                                if(newS.pos >= p.length())
                                    return true;
                                stateStack.add(newS);
                                currMatched = newS.nextMatched;
                                i = newS.pos;
                                break;
                            }
                        }
                        continue;
                    }else
                        break;
                }
                if(currMatched >= s.length()){
                    if(p.charAt(i) == '*'){
                        i++;
                        continue;
                    }
                    break;
                }
                if(p.charAt(i)==s.charAt(currMatched) || p.charAt(i)=='?'){
                    i++;
                    currMatched++;
                }else if(p.charAt(i)=='*'){
                    State state = findNextMatched(s, currMatched, p, i);
                    if(state != null) {
                        if(state.pos >= p.length())
                            return true;
                        stateStack.add(state);
                        currMatched = state.nextMatched;
                        i = state.pos;
                    }else {
                        i++;
//                        currMatched++;
                    }
                }else {
                    if(!stateStack.empty()) {
                        while (!stateStack.empty()) {
                            State pre = stateStack.pop();
                            State newS = back(s, p, pre);
                            if (newS != null) {
                                if (newS.pos >= p.length())
                                    return true;
                                stateStack.add(newS);
                                currMatched = newS.nextMatched;
                                i = newS.pos;
                                break;
                            }
                        }
                    }else
                    return false;
                }
            }
            return currMatched==s.length() && i==p.length();
        }

        State back(String s, String p, State pre){
            return findNextMatched(s, pre.nextMatched+1, p, pre.pos-1);
        }

        State findNextMatched(String s, int curMatched, String p, int i){
            while (i<p.length() && p.charAt(i)=='*'){
                i++;
            }
            if(i >= p.length()){
                return new State(i, s.length());
            }
            while (curMatched<s.length()){
                if(s.charAt(curMatched)==p.charAt(i)||p.charAt(i)=='?'){
                    return new State(i, curMatched);
                }
                curMatched++;
            }
            return null;
        }
    }


    @Test
    public void test0(){
        String s = "abefcdgiescdfimde";
        String p = "ab*cd?i*de";
        Solution solution = new Solution();
        Assert.assertTrue(solution.isMatch(s, p));
    }

    @Test
    public void test1(){
        String s = "abcde";
        String p = "ab?de";
        Solution solution = new Solution();
        Assert.assertTrue(solution.isMatch(s, p));
    }

    @Test
    public void test3(){
        String s = "abckkkkde";
        String p = "ab*de";
        Solution solution = new Solution();
        Assert.assertTrue(solution.isMatch(s, p));
    }

    @Test
    public void test4(){
        String s = "abckkkkdef";
        String p = "ab*de";
        Solution solution = new Solution();
        Assert.assertFalse(solution.isMatch(s, p));
    }

    @Test
    public void test5(){
        String s = "abckkkkfef";
        String p = "ab*de";
        Solution solution = new Solution();
        Assert.assertFalse(solution.isMatch(s, p));
    }


    @Test
    public void test2(){
        String s = "abcde";
        String p = "ab?cde";
        Solution solution = new Solution();
        Assert.assertFalse(solution.isMatch(s, p));
    }

    @Test
    public void test6(){
        String s = "aa";
        String p = "*";
        Solution solution = new Solution();
        Assert.assertTrue(solution.isMatch(s, p));
    }

    @Test
    public void test7() {
        String s = "acdcb";
        String p = "a*c?b";
        Solution solution = new Solution();
        Assert.assertFalse(solution.isMatch(s, p));
    }

    @Test
    public void test8() {
        String s = "mississippi";
        String p = "m??*ss*?i*pi";
        Solution solution = new Solution();
        Assert.assertFalse(solution.isMatch(s, p));
    }

    @Test
    public void test9() {
        String s = "aaa";
        String p = "***a";
        Solution solution = new Solution();
        Assert.assertTrue(solution.isMatch(s, p));
    }

    @Test
    public void test10() {
        String s = "";
        String p = "*";
        Solution solution = new Solution();
        Assert.assertTrue(solution.isMatch(s, p));
    }


    @Test
    public void test11() {
        String s = "cdd";
        String p = "*d";
        Solution solution = new Solution();
        Assert.assertTrue(solution.isMatch(s, p));
    }

    @Test
    public void test12() {
        String s = "a";
        String p = "a*";
        Solution solution = new Solution();
        Assert.assertTrue(solution.isMatch(s, p));
    }

    @Test
    public void test15() {
        String s = "";
        String p = "*a";
        Solution solution = new Solution();
        Assert.assertFalse(solution.isMatch(s, p));
    }



    @Test
    public void test13() {
        String s = "hi";
        String p = "*?";
        Solution solution = new Solution();
        Assert.assertTrue(solution.isMatch(s, p));
    }


    @Test
    public void test14() {
        String s = "ab";
        String p = "*?*?*";
        Solution solution = new Solution();
        Assert.assertTrue(solution.isMatch(s, p));
    }

}


