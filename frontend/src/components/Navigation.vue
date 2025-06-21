<script setup lang="ts">
import { ref, onMounted, onUnmounted } from 'vue'
import { useRouter } from 'vue-router'
// import { useUserStore } from '../stores/user'

const router = useRouter()
// const userStore = useUserStore()

const isMenuOpen = ref(false)
const handleResize = () => {
  if (window.innerWidth > 768) isMenuOpen.value = false
}
onMounted(() => window.addEventListener('resize', handleResize))
onUnmounted(() => window.removeEventListener('resize', handleResize))

const go = (path: string) => {
  router.push(path)
  isMenuOpen.value = false
}
</script>

<template>
  <nav class="nav">
    <div class="nav-logo" @click="go('/')">
      <i class="pi pi-calendar-plus"></i>
      <span class="brand-text">SpecialMoment</span>
    </div>
    
    <!-- Desktop Menu -->
    <ul class="nav-links desktop-menu">
      <li>
        <a @click.prevent="go('/')" :class="{active: $route.path === '/'}">
          <i class="pi pi-home"></i>
          <span class="nav-label">Acasă</span>
        </a>
      </li>
      <li>
        <a @click.prevent="go('/events')" :class="{active: $route.path === '/events'}">
          <i class="pi pi-calendar"></i>
          <span class="nav-label">Evenimente</span>
        </a>
      </li>
      <li>
        <a @click.prevent="go('/profile')" :class="{active: $route.path === '/profile'}">
          <i class="pi pi-user"></i>
          <span class="nav-label">Profil</span>
        </a>
      </li>
    </ul>

    <!-- Mobile Menu Icons -->
    <div class="mobile-menu">
      <a @click.prevent="go('/')" :class="{active: $route.path === '/'}">
        <i class="pi pi-home"></i>
      </a>
      <a @click.prevent="go('/events')" :class="{active: $route.path === '/events'}">
        <i class="pi pi-calendar"></i>
      </a>
      <a @click.prevent="go('/profile')" :class="{active: $route.path === '/profile'}">
        <i class="pi pi-user"></i>
      </a>
    </div>
  </nav>
</template>

<style scoped>
.nav {
  display: flex;
  align-items: center;
  justify-content: space-between;
  background: linear-gradient(90deg, #ff6b9d 0%, #f8b5d3 100%);
  padding: 0.5rem 1.5rem;
  border-bottom: 1px solid #f8b5d3;
  position: sticky;
  top: 0;
  z-index: 100;
  box-shadow: 0 2px 12px 0 rgba(196, 69, 105, 0.07);
}

.nav-logo {
  display: flex;
  align-items: center;
  gap: 0.5rem;
  font-weight: bold;
  font-size: 1.3rem;
  cursor: pointer;
  color: #fff;
  font-family: 'Julee', cursive;
  text-shadow: 0 1px 2px rgba(0,0,0,0.1);
}

.nav-logo i {
  font-size: 2rem;
  color: #fff;
  text-shadow: 0 1px 2px rgba(0,0,0,0.1);
}

.brand-text {
  font-size: 1.3rem;
  font-weight: 700;
  letter-spacing: 1px;
}

/* Desktop Menu */
.desktop-menu {
  display: flex;
  gap: 2rem;
  list-style: none;
  margin: 0;
  padding: 0;
  align-items: center;
}

.desktop-menu li a {
  display: flex;
  align-items: center;
  gap: 0.6rem;
  text-decoration: none;
  color: #c44569;
  font-weight: 600;
  font-size: 1.1rem;
  padding: 0.6rem 1.2rem;
  border-radius: 12px;
  transition: background 0.2s, color 0.2s, box-shadow 0.2s;
  position: relative;
  cursor: pointer;
}

.desktop-menu li a.active,
.desktop-menu li a:hover {
  background: rgba(255,255,255,0.25);
  color: #fff;
  box-shadow: 0 2px 8px 0 rgba(196, 69, 105, 0.10);
}

.desktop-menu li a i {
  font-size: 1.3rem;
  transition: color 0.2s;
}

.desktop-menu li a.active i,
.desktop-menu li a:hover i {
  color: #fff;
}

.nav-label {
  display: inline;
}

/* Mobile Menu */
.mobile-menu {
  display: none;
  align-items: center;
  gap: 1.5rem;
}

.mobile-menu a {
  display: flex;
  align-items: center;
  justify-content: center;
  width: 40px;
  height: 40px;
  border-radius: 50%;
  color: #c44569;
  text-decoration: none;
  transition: all 0.2s ease;
  cursor: pointer;
}

.mobile-menu a i {
  font-size: 1.5rem;
  transition: color 0.2s;
}

.mobile-menu a.active,
.mobile-menu a:hover {
  background: rgba(255,255,255,0.25);
  color: #fff;
  transform: scale(1.1);
}

.mobile-menu a.active i,
.mobile-menu a:hover i {
  color: #fff;
}

@media (max-width: 768px) {
  .brand-text {
    display: none;
  }
  
  .desktop-menu {
    display: none;
  }
  
  .mobile-menu {
    display: flex;
  }
}
</style> 