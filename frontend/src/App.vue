<script setup lang="ts">
import Navigation from './components/Navigation.vue'
</script>

<template>
  <div class="layout">
    <Navigation />
    <router-view></router-view>
  </div>
</template>

<style>
body {
  margin: 0;
  padding: 0;
  font-family: var(--font-family);
  width: 100%;
  height: 100%;
  overflow-x: hidden;
}

.layout {
  display: flex;
  flex-direction: column;
  width: 100%;
  height: 100%;
  min-height: 100vh;
}
.layout-content {
  flex: 1;
}
</style>
