import { defineStore } from 'pinia'

export const useCounterStore = defineStore('counter', {
  state: () => ({
    count: 0,
    name: 'Contor'
  }),
  
  getters: {
    doubleCount: (state) => state.count * 2,
    getCountWithName: (state) => `${state.name}: ${state.count}`
  },
  
  actions: {
    increment() {
      this.count++
    },
    decrement() {
      this.count--
    },
    reset() {
      this.count = 0
    },
    incrementBy(amount: number) {
      this.count += amount
    }
  }
}) 