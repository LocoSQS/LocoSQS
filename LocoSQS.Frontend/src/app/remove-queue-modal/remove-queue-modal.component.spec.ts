import { ComponentFixture, TestBed } from '@angular/core/testing';

import { RemoveQueueModalComponent } from './remove-queue-modal.component';

describe('RemoveQueueModalComponent', () => {
  let component: RemoveQueueModalComponent;
  let fixture: ComponentFixture<RemoveQueueModalComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [RemoveQueueModalComponent]
    });
    fixture = TestBed.createComponent(RemoveQueueModalComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
