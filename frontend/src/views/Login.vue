<template>
    <div class="login-container">
      <div class="login-background">
        <div class="background-overlay"></div>
        <div class="floating-elements">
          <div class="floating-heart">💕</div>
          <div class="floating-star">✨</div>
          <div class="floating-flower">🌸</div>
          <div class="floating-heart-2">💖</div>
          <div class="floating-star-2">⭐</div>
        </div>
      </div>
      
      <div class="login-card">
        <div class="login-header">
          <div class="logo-container">
            <i class="pi pi-calendar-plus logo-icon"></i>
          </div>
          <h1 class="app-title">SpecialMoment</h1>
          <p class="app-subtitle">Planifică evenimentele tale speciale</p>
        </div>
        
        <form @submit.prevent="handleLogin" class="login-form">
          <div v-if="successMessage" class="success-alert">
            <i class="pi pi-check-circle"></i>
            {{ successMessage }}
          </div>
          
          <div class="form-group">
            <label for="email" class="form-label">Email</label>
            <div class="input-wrapper">
              <i class="pi pi-envelope input-icon"></i>
              <InputText
                id="email"
                v-model="email"
                type="email"
                placeholder="Introduceți email-ul"
                class="login-input"
                :class="{ 'p-invalid': errors.email }"
                :disabled="userStore.isLoading"
              />
            </div>
            <small v-if="errors.email" class="error-message">{{ errors.email }}</small>
          </div>
          
          <div class="form-group">
            <label for="password" class="form-label">Parolă</label>
            <div class="input-wrapper">
              <i class="pi pi-lock input-icon"></i>
              <Password
                id="password"
                v-model="password"
                placeholder="Introduceți parola"
                class="login-input"
                :class="{ 'p-invalid': errors.password }"
                :disabled="userStore.isLoading"
                :feedback="false"
                toggleMask
              />
            </div>
            <small v-if="errors.password" class="error-message">{{ errors.password }}</small>
          </div>
          
          <div class="form-options">
            <div class="remember-me">
              <Checkbox
                v-model="rememberMe"
                :binary="true"
                inputId="remember"
              />
              <label for="remember" class="checkbox-label">Ține-mă minte</label>
            </div>
            <a href="#" class="forgot-password">Ai uitat parola?</a>
          </div>
          
          <Button
            type="submit"
            :loading="userStore.isLoading"
            :disabled="!isFormValid"
            class="login-button"
            severity="primary"
          >
            <i class="pi pi-sign-in" v-if="!userStore.isLoading"></i>
            {{ userStore.isLoading ? 'Se conectează...' : 'Conectare' }}
          </Button>
          
          <div v-if="loginError" class="error-alert">
            <i class="pi pi-exclamation-triangle"></i>
            {{ loginError }}
          </div>
        </form>
        
        <div class="login-footer">
          <p class="register-text">
            Nu ai cont? 
            <a href="#" @click.prevent="goToRegister" class="register-link">Înregistrează-te</a>
          </p>
        </div>
      </div>
    </div>
  </template>
  
  <script setup lang="ts">
  import { useUserStore } from '../stores/user'
  import { ref, computed, onMounted } from 'vue'
  import { useRouter, useRoute } from 'vue-router'
  const router = useRouter()
  const userStore = useUserStore()
  const route = useRoute()
  
  const email = ref('')
  const password = ref('')
  const rememberMe = ref(false)
  const loginError = ref('')
  const successMessage = ref('')
  
  const errors = ref({
    email: '',
    password: ''
  })
  
  // Verifică dacă utilizatorul vine de la înregistrare
  onMounted(() => {
    if (route.query.registered === 'true') {
      successMessage.value = 'Contul a fost creat cu succes! Te poți conecta acum.'
    }
  })
  
  const isFormValid = computed(() => {
    return email.value.trim() && password.value.trim() && !errors.value.email && !errors.value.password
  })
  
  const validateEmail = (email: string) => {
    const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/
    return emailRegex.test(email)
  }
  
  const validateForm = () => {
    errors.value = { email: '', password: '' }
    
    if (!email.value.trim()) {
      errors.value.email = 'Email-ul este obligatoriu'
    } else if (!validateEmail(email.value)) {
      errors.value.email = 'Email-ul nu este valid'
    }
    
    if (!password.value.trim()) {
      errors.value.password = 'Parola este obligatorie'
    } else if (password.value.length < 6) {
      errors.value.password = 'Parola trebuie să aibă cel puțin 6 caractere'
    }
    
    return !errors.value.email && !errors.value.password
  }
  
  const handleLogin = async () => {
    if (!validateForm()) return
    
    loginError.value = ''
    
    try {
      await userStore.login(email.value, password.value)
      router.push('/dashboard')
    } catch (error: any) {
      loginError.value = error.message || 'Eroare la conectare. Încercați din nou.'
    }
  }
  
  const goToRegister = () => {
    router.push('/register')
  }
  </script>
  
  <style scoped>
  .login-container {
    min-height: 100vh;
    width: 100vw;
    display: flex;
    align-items: center;
    justify-content: center;
    position: relative;
    padding: 0;
    overflow: hidden;
  }
  
  .login-background {
    position: absolute;
    top: 0;
    left: 0;
    right: 0;
    bottom: 0;
    background: linear-gradient(135deg, #ff6b9d 0%, #c44569 25%, #f8b5d3 50%, #ff9ff3 75%, #f368e0 100%);
    z-index: -2;
  }
  
  .background-overlay {
    position: absolute;
    top: 0;
    left: 0;
    right: 0;
    bottom: 0;
    background: rgba(255, 255, 255, 0.1);
    z-index: -1;
  }
  
  .floating-elements {
    position: absolute;
    top: 0;
    left: 0;
    right: 0;
    bottom: 0;
    pointer-events: none;
    z-index: -1;
  }
  
  .floating-heart, .floating-star, .floating-flower, .floating-heart-2, .floating-star-2 {
    position: absolute;
    font-size: 2rem;
    animation: float 6s ease-in-out infinite;
    opacity: 0.3;
  }
  
  .floating-heart {
    top: 10%;
    left: 10%;
    animation-delay: 0s;
  }
  
  .floating-star {
    top: 20%;
    right: 15%;
    animation-delay: 1s;
  }
  
  .floating-flower {
    bottom: 20%;
    left: 15%;
    animation-delay: 2s;
  }
  
  .floating-heart-2 {
    bottom: 10%;
    right: 10%;
    animation-delay: 3s;
  }
  
  .floating-star-2 {
    top: 50%;
    left: 5%;
    animation-delay: 4s;
  }
  
  @keyframes float {
    0%, 100% { transform: translateY(0px) rotate(0deg); }
    50% { transform: translateY(-20px) rotate(180deg); }
  }
  
  .login-card {
    background: rgba(255, 255, 255, 0.95);
    border-radius: 25px;
    box-shadow: 0 25px 50px rgba(196, 69, 105, 0.2);
    padding: 3rem;
    width: 100%;
    max-width: 450px;
    backdrop-filter: blur(20px);
    border: 1px solid rgba(255, 255, 255, 0.3);
    position: relative;
    z-index: 1;
  }
  
  .login-card::before {
    content: '';
    position: absolute;
    top: -2px;
    left: -2px;
    right: -2px;
    bottom: -2px;
    background: linear-gradient(45deg, #ff6b9d, #c44569, #f8b5d3, #ff9ff3);
    border-radius: 25px;
    z-index: -1;
    opacity: 0.3;
  }
  
  .login-header {
    text-align: center;
    margin-bottom: 2.5rem;
  }
  
  .logo-container {
    margin-bottom: 1.5rem;
  }
  
  .logo-icon {
    font-size: 4rem;
    color: #c44569;
    background: linear-gradient(135deg, #ff6b9d, #c44569);
    -webkit-background-clip: text;
    -webkit-text-fill-color: transparent;
    background-clip: text;
    filter: drop-shadow(0 4px 8px rgba(196, 69, 105, 0.3));
  }
  
  .app-title {
    font-size: 2.5rem;
    font-weight: 700;
    color: #2c3e50;
    margin: 0 0 0.5rem 0;
    font-family: 'Julee', cursive;
    background: linear-gradient(135deg, #c44569, #ff6b9d);
    -webkit-background-clip: text;
    -webkit-text-fill-color: transparent;
    background-clip: text;
  }
  
  .app-subtitle {
    color: #7f8c8d;
    margin: 0;
    font-size: 1.1rem;
    font-weight: 500;
  }
  
  .login-form {
    margin-bottom: 2rem;
  }
  
  .form-group {
    margin-bottom: 1.8rem;
  }
  
  .form-label {
    display: block;
    margin-bottom: 0.8rem;
    font-weight: 600;
    color: #2c3e50;
    font-size: 1rem;
  }
  
  .input-wrapper {
    position: relative;
  }
  
  .input-icon {
    position: absolute;
    left: 15px;
    top: 50%;
    transform: translateY(-50%);
    color: #c44569;
    z-index: 1;
    font-size: 1.1rem;
  }
  
  .login-input {
    width: 100%;
    padding: 15px 15px 15px 50px;
    border: 2px solid #f8b5d3;
    border-radius: 15px;
    font-size: 1rem;
    transition: all 0.3s ease;
    background: rgba(255, 255, 255, 0.9);
  }
  
  .login-input:focus {
    border-color: #c44569;
    box-shadow: 0 0 0 4px rgba(196, 69, 105, 0.1);
    background: white;
  }
  
  .login-input.p-invalid {
    border-color: #e74c3c;
  }
  
  .error-message {
    color: #e74c3c;
    font-size: 0.85rem;
    margin-top: 0.5rem;
    display: block;
    font-weight: 500;
  }
  
  .form-options {
    display: flex;
    justify-content: space-between;
    align-items: center;
    margin-bottom: 2rem;
    flex-wrap: wrap;
    gap: 1rem;
  }
  
  .remember-me {
    display: flex;
    align-items: center;
    gap: 0.8rem;
  }
  
  .checkbox-label {
    font-size: 0.95rem;
    color: #7f8c8d;
    cursor: pointer;
    font-weight: 500;
  }
  
  .forgot-password {
    color: #c44569;
    text-decoration: none;
    font-size: 0.95rem;
    font-weight: 600;
    transition: color 0.3s ease;
  }
  
  .forgot-password:hover {
    color: #ff6b9d;
    text-decoration: underline;
  }
  
  .login-button {
    width: 100%;
    padding: 15px;
    font-size: 1.1rem;
    font-weight: 600;
    border-radius: 15px;
    background: linear-gradient(135deg, #c44569, #ff6b9d);
    border: none;
    color: white;
    cursor: pointer;
    transition: all 0.3s ease;
    display: flex;
    align-items: center;
    justify-content: center;
    gap: 0.8rem;
    box-shadow: 0 8px 20px rgba(196, 69, 105, 0.3);
  }
  
  .login-button:hover:not(:disabled) {
    transform: translateY(-3px);
    box-shadow: 0 12px 25px rgba(196, 69, 105, 0.4);
    background: linear-gradient(135deg, #ff6b9d, #c44569);
  }
  
  .login-button:disabled {
    opacity: 0.6;
    cursor: not-allowed;
    transform: none;
  }
  
  .error-alert {
    background: linear-gradient(135deg, #ffe6e6, #ffcccc);
    color: #e74c3c;
    padding: 15px;
    border-radius: 12px;
    margin-top: 1.5rem;
    display: flex;
    align-items: center;
    gap: 0.8rem;
    font-size: 0.95rem;
    border: 1px solid #ffb3b3;
  }
  
  .success-alert {
    background: linear-gradient(135deg, #e6ffe6, #ccffcc);
    color: #27ae60;
    padding: 15px;
    border-radius: 12px;
    margin-bottom: 1.5rem;
    display: flex;
    align-items: center;
    gap: 0.8rem;
    font-size: 0.95rem;
    border: 1px solid #b3ffb3;
  }
  
  .login-footer {
    text-align: center;
    padding-top: 2rem;
    border-top: 2px solid #f8b5d3;
  }
  
  .register-text {
    color: #7f8c8d;
    margin: 0;
    font-size: 1rem;
    font-weight: 500;
  }
  
  .register-link {
    color: #c44569;
    text-decoration: none;
    font-weight: 700;
    transition: color 0.3s ease;
  }
  
  .register-link:hover {
    color: #ff6b9d;
    text-decoration: underline;
  }
  
  /* Responsive Design */
  @media (max-width: 768px) {
    .login-container {
      padding: 0;
      min-height: 100vh;
    }
    
    .login-card {
      padding: 2.5rem 2rem;
      border-radius: 20px;
      max-width: 100%;
      margin: 0 1rem;
    }
    
    .app-title {
      font-size: 2.2rem;
    }
    
    .logo-icon {
      font-size: 3.5rem;
    }
    
    .form-options {
      flex-direction: column;
      align-items: flex-start;
      gap: 1.2rem;
    }
    
    .forgot-password {
      align-self: flex-end;
    }
    
    .floating-elements {
      display: none;
    }
  }
  
  @media (max-width: 480px) {
    .login-container {
      padding: 0;
    }
    
    .login-card {
      padding: 2rem 1.5rem;
      border-radius: 15px;
      margin: 0 0.5rem;
    }
    
    .app-title {
      font-size: 2rem;
    }
    
    .logo-icon {
      font-size: 3rem;
    }
    
    .login-input {
      padding: 12px 12px 12px 45px;
    }
    
    .input-icon {
      left: 12px;
    }
    
    .login-button {
      padding: 12px;
      font-size: 1rem;
    }
  }
  
  @media (min-width: 1200px) {
    .login-card {
      max-width: 500px;
      padding: 3.5rem;
    }
    
    .app-title {
      font-size: 3rem;
    }
    
    .logo-icon {
      font-size: 4.5rem;
    }
  }
  
  /* Landscape orientation for mobile */
  @media (max-height: 600px) and (orientation: landscape) {
    .login-container {
      padding: 0;
    }
    
    .login-card {
      padding: 2rem;
      max-height: 90vh;
      overflow-y: auto;
    }
    
    .login-header {
      margin-bottom: 1.5rem;
    }
    
    .form-group {
      margin-bottom: 1rem;
    }
  }
  </style>  