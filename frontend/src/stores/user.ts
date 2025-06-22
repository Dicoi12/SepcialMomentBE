import { defineStore } from "pinia";
import fetchApi from "../fetch";

interface User {
  id: string;
  name: string;
  email: string;
  isAuthenticated: boolean;
}

interface LoginRequest {
  email: string;
  password: string;
}

interface RegisterRequest {
  name: string;
  email: string;
  password: string;
}

interface LoginResponse {
  token: string;
  id: string;
  name: string;
  email: string;
}

interface RegisterResponse {
  message: string;
  user: {
    id: string;
    name: string;
    email: string;
  };
}

export const useUserStore = defineStore("user", {
  state: (): {
    user: User | null;
    isLoading: boolean;
    token: string | null;
  } => ({
    user: null,
    isLoading: false,
    token: localStorage.getItem("token"),
  }),

  getters: {
    isLoggedIn: (state) => state.user?.isAuthenticated ?? false,
    userName: (state) => state.user?.name ?? "Utilizator necunoscut",
    userEmail: (state) => state.user?.email ?? "",
  },

  actions: {
    async register(name: string, email: string, password: string) {
      this.isLoading = true;
      try {
        const registerData: RegisterRequest = { name, email, password };
        const response: RegisterResponse = await fetchApi(
          "Auth/register",
          "POST",
          registerData
        );

        return response;
      } catch (error: any) {
        console.error("Eroare la înregistrare:", error);
        throw new Error(error.message || "Eroare la înregistrare");
      } finally {
        this.isLoading = false;
      }
    },

    async login(email: string, password: string) {
      this.isLoading = true;
      try {
        const loginData: LoginRequest = { email, password };
        const response: LoginResponse = await fetchApi(
          "Auth/login",
          "POST",
          loginData
        );

        // Salvează token-ul
        this.token = response.token;
        localStorage.setItem("token", response.token);

        // Actualizează user-ul
        this.user = {
          id: response.id,
          name: response.name,
          email: response.email,
          isAuthenticated: true,
        };
      } catch (error: any) {
        console.error("Eroare la login:", error);
        throw new Error(error.message || "Eroare la autentificare");
      } finally {
        this.isLoading = false;
      }
    },

    async logout() {
      try {
        // Opțional: trimite cerere de logout către server
        if (this.token) {
          await fetchApi("Auth/logout", "POST");
        }
      } catch (error) {
        console.error("Eroare la logout:", error);
      } finally {
        // Curăță datele locale
        this.user = null;
        this.token = null;
        localStorage.removeItem("token");
      }
    },

    async checkAuth() {
      if (!this.token) return false;

      try {
        const user = await fetchApi("Auth/me", "GET");
        this.user = {
          id: user.id,
          name: user.name,
          email: user.email,
          isAuthenticated: true,
        };
        return true;
      } catch (error) {
        this.logout();
        return false;
      }
    },

    updateProfile(name: string) {
      if (this.user) {
        this.user.name = name;
      }
    },
  },
});
