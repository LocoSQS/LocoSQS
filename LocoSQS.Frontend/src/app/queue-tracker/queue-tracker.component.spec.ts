import { ComponentFixture, TestBed } from '@angular/core/testing';

import { QueueTrackerComponent } from './queue-tracker.component';

describe('QueueTrackerComponent', () => {
  let component: QueueTrackerComponent;
  let fixture: ComponentFixture<QueueTrackerComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [QueueTrackerComponent]
    });
    fixture = TestBed.createComponent(QueueTrackerComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
