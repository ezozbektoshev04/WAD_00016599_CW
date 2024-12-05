// 00016599
import { HttpClient, HttpClientModule } from '@angular/common/http';
import { Component, inject, OnInit } from '@angular/core';
import {
  FormControl,
  FormGroup,
  FormsModule,
  ReactiveFormsModule,
} from '@angular/forms';
import { Observable } from 'rxjs';
import { Feedback } from '../models/feedback.model';
import { User } from '../models/user.model';
import { AsyncPipe } from '@angular/common';

@Component({
  selector: 'app-root',
  imports: [HttpClientModule, FormsModule, ReactiveFormsModule, AsyncPipe],
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css'],
})
export class AppComponent implements OnInit {
  //00016599
  private readonly http = inject(HttpClient);

  feedbacksForm = new FormGroup({
    feedbackId: new FormControl<number>(0),
    feedbackTitle: new FormControl<string>(''),
    feedbackDescription: new FormControl<string>(''),
    userId: new FormControl<number>(0),
  });
  //00016599
  feedback$!: Observable<Feedback[]>;
  user$!: Observable<User[]>;

  ngOnInit(): void {
    this.feedback$ = this.loadFeedbacks();
    this.user$ = this.getUsers();
  }

  //00016599
  private getUsers(): Observable<User[]> {
    return this.http.get<User[]>('https://localhost:7006/api/Users');
  }

  //00016599
  private loadFeedbacks(): Observable<Feedback[]> {
    return this.http.get<Feedback[]>('https://localhost:7006/api/Feedbacks');
  }
  //00016599

  onFormSubmit(): void {
    const feedbackData = this.feedbacksForm.value;
    const apiRequest = this.createApiRequest(feedbackData);

    if (feedbackData.feedbackId === 0 || !feedbackData.feedbackId) {
      this.createFeedback(apiRequest);
    } else {
      this.updateFeedback(feedbackData.feedbackId ?? 0, apiRequest);
    }
  }

  private createApiRequest(feedbackData: any): object {
    return {
      feedbackTitle: feedbackData.feedbackTitle,
      feedbackDescription: feedbackData.feedbackDescription,
      userId: feedbackData.userId,
    };
  }

  //00016599
  private createFeedback(feedbackRequest: object): void {
    this.http
      .post('https://localhost:7006/api/Feedbacks', feedbackRequest)
      .subscribe({
        next: () => {
          this.refreshFeedbacks();
          this.feedbacksForm.reset();
        },
        error: (error) => {
          console.error('Error creating feedback:', error);
          alert('Error creating feedback');
        },
      });
  }

  private updateFeedback(feedbackId: number, feedbackRequest: object): void {
    this.http
      .put(
        `https://localhost:7006/api/Feedbacks/${feedbackId}`,
        feedbackRequest
      )
      .subscribe({
        next: () => {
          this.refreshFeedbacks();
          this.feedbacksForm.reset();
        },
        error: (error) => {
          console.error('Error updating feedback:', error);
          alert('Error updating feedback');
        },
      });
  }

  // 00016599
  onDelete(feedbackId: number): void {
    this.http
      .delete(`https://localhost:7006/api/Feedbacks/${feedbackId}`)
      .subscribe({
        next: () => {
          alert('Item Deleted');
          this.refreshFeedbacks();
        },
        error: (error) => {
          console.error('Error deleting feedback:', error);
          alert('Error deleting feedback');
        },
      });
  }

  private refreshFeedbacks(): void {
    this.feedback$ = this.loadFeedbacks();
  }

  //00016599
  onEdit(feedback: Feedback): void {
    this.feedbacksForm.patchValue({
      feedbackId: feedback.feedbackId,
      feedbackTitle: feedback.feedbackTitle,
      feedbackDescription: feedback.feedbackDescription,
      userId: feedback.userId,
    });
  }

  title = 'FeedbackApp';
}
