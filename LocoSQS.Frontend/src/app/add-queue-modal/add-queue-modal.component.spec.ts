import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AddQueueModalComponent } from './add-queue-modal.component';

describe('AddQueueModalComponent', () => {
  let component: AddQueueModalComponent;
  let fixture: ComponentFixture<AddQueueModalComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [AddQueueModalComponent]
    });
    fixture = TestBed.createComponent(AddQueueModalComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
