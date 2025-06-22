export interface Event {
  id: number;
  title: string;
  eventType: string;
  date: Date | string;
  location: string;
  description: string;
  userId: number;
  formData?: Record<string, any>;
  weddingGuests?: any[];
  tables?: any[];
  weddingExpenses?: any[];
}

export interface EventFormField {
  name: string;
  label: string;
  type: 'text' | 'textarea' | 'number' | 'dropdown' | 'checkbox' | 'date';
  required: boolean;
  placeholder?: string;
  defaultValue?: any;
  options?: Array<{ label: string; value: any }>;
  rows?: number;
  validation?: {
    min?: number;
    max?: number;
    pattern?: string;
    message?: string;
  };
}

export interface EventFormTemplate {
  id: number;
  eventType: string;
  name: string;
  description: string;
  fields: EventFormField[];
  isActive: boolean;
  createdAt: Date | string;
  updatedAt: Date | string;
}

export interface CreateEventRequest {
  title: string;
  eventType: string;
  date: Date;
  location: string;
  description: string;
  userId: number;
  formData?: Record<string, any>;
}

export interface UpdateEventRequest extends CreateEventRequest {
  id: number;
}

export type EventType = 'Wedding' | 'Baptism' | 'Anniversary' | 'Birthday' | 'Other'; 